using System.ComponentModel.DataAnnotations;

namespace Reddit.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }
        public virtual List<Comment> Comments { get; set; } = new List<Comment>();
        public virtual User Author { get; set; }
        public int AuthorId { get; set; }
        public int Upvote { get; set; }
        public int Downvote { get; set; }
        public int CommunityId { get; set; }
        public virtual Community Community { get; set; }

    }
}