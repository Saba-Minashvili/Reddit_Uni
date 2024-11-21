using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reddit.Dtos;
using Reddit.Models;
using Reddit.Repositories;
using System.Linq.Expressions;

namespace Reddit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Posts1Controller : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IPostsRepository _postsRepository;

        public Posts1Controller(ApplicationDbContext context, IPostsRepository postsRepository)
        {
            _context = context;
            _postsRepository = postsRepository;
        }

        // GET: api/Posts1
        [HttpGet]
        public async Task<PagedList<Post>> GetPosts(
            string? searchTerm = null,
            int pageSize=3, int pageNumber = 1,
            string? sortTerm = null, bool isAscending = true)
        {
            return await _postsRepository.GetPosts(pageNumber, pageSize, searchTerm, sortTerm, isAscending);
        }

[HttpPost("Upvote")]
public async Task<IActionResult> UpvoteAsync(int postId)
{
        var post = await _context.Posts.FindAsync(postId);
        
        if (post == null)
        {
            return NotFound(new { message = "Post not found" });
        }
        post.Upvote += 1;
        await _context.SaveChangesAsync();
        return Ok(new { message = "Upvoted successfully", upvotes = post.Upvote });
    }

        [HttpPost("Downvote")]
        public async Task<IActionResult> DownvoteAsync(int postId)
        {
            var post = await _context.Posts.FindAsync(postId);

            if (post == null)
            {
                return NotFound(new { message = "Post not found" });
            }
            post.Downvote += 1;
            await _context.SaveChangesAsync();
            return Ok(new { message = "Downvoted successfully", downvotes = post.Downvote });
        }


    

        // GET: api/Posts1/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetPost(int id)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == id); // .Include(p => p.Comments)

            if (post == null)
            {
                return NotFound();
            }

            return post;
        }

        // PUT: api/Posts1/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPost(int id, Post post)
        {
            if (id != post.Id)
            {
                return BadRequest();
            }

            _context.Entry(post).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Posts1
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Post>> PostPost(PostDto postDto)
        {
            var post = postDto.CreatePost();

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPost", new { id = post.Id }, post);
        }

        // DELETE: api/Posts1/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }
    }
}
