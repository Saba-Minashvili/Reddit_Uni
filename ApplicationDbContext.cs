using Microsoft.EntityFrameworkCore;
using Reddit.Models;

namespace Reddit
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Community> Communities { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
            base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Post-Comment Relationship
            modelBuilder.Entity<Comment>()
                          .HasOne(c => c.Post)
                          .WithMany(p => p.Comments)
                          .HasForeignKey(c => c.PostId)
                          .OnDelete(DeleteBehavior.Cascade);

            // Post-Community Relationship
            modelBuilder.Entity<Post>(p =>
            {
                p.HasMany(p => p.Comments)
                 .WithOne(c => c.Post)
                 .HasForeignKey(c => c.PostId)
                 .OnDelete(DeleteBehavior.Cascade);

                p.HasOne(p => p.Author)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.AuthorId)
                .OnDelete(DeleteBehavior.SetNull);

                p.HasOne(p => p.Community)
                .WithMany(c => c.Posts)
                .HasForeignKey(p => p.CommunityId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            // Comment Entity Configuration
            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.Property(c => c.Content)
                    .IsRequired();

                entity.HasOne(c => c.Author)
                    .WithMany(u => u.Comments)
                    .HasForeignKey(c => c.AuthorId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // User Entity Configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);

                entity.Property(u => u.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(u => u.Email)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.HasMany(u => u.Posts)
                    .WithOne(p => p.Author)
                    .HasForeignKey(p => p.AuthorId);

                entity.HasMany(u => u.Comments)
                    .WithOne(c => c.Author)
                    .HasForeignKey(c => c.AuthorId);

                // User-Community Relationship (Ownership)
                entity.HasMany(u => u.OwnedCommunities)
                      .WithOne(c => c.Owner)
                      .HasForeignKey(c => c.OwnerId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Community Entity Configuration
            modelBuilder.Entity<Community>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.Property(c => c.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(c => c.Description)
                    .HasMaxLength(500);

                // Relationship between Community and Users (Subscribers)
                entity.HasMany(c => c.Subscribers)
                      .WithMany(u => u.SubscribedCommunities)
                      .UsingEntity(j => j.ToTable("UserCommunitySubscriptions"));

                // Relationship between Community and Posts
                entity.HasMany(c => c.Posts)
                      .WithOne(p => p.Community)
                      .HasForeignKey(p => p.CommunityId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
