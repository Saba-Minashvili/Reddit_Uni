using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reddit.Dtos;
using Reddit.Models;

namespace Reddit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommunitiesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CommunitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Communities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Community>>> GetCommunities(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? sortKey = "id",
            [FromQuery] bool? isAscending = true,
            [FromQuery] string? searchKey = null)
        {
            var query = _context.Communities.AsQueryable();

            // Apply searching
            if (!string.IsNullOrEmpty(searchKey))
            {
                query = query.Where(c => c.Name.Contains(searchKey) || c.Description.Contains(searchKey));
            }

            // Apply sorting
            query = sortKey?.ToLower() switch
            {
                "createdat" => isAscending == true
                    ? query.OrderBy(c => c.CreatedAt)
                    : query.OrderByDescending(c => c.CreatedAt),
                "postscount" => isAscending == true
                    ? query.OrderBy(c => c.Posts.Count)
                    : query.OrderByDescending(c => c.Posts.Count),
                "subscriberscount" => isAscending == true
                    ? query.OrderBy(c => c.Subscribers.Count)
                    : query.OrderByDescending(c => c.Subscribers.Count),
                _ => isAscending == true
                    ? query.OrderBy(c => c.Id)
                    : query.OrderByDescending(c => c.Id)
            };

            // Apply pagination
            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            // Execute query and return results
            var result = await query
                .Select(c => new
                {
                    c.Id,
                    c.Name,
                    c.Description,
                    CreatedAt = c.CreatedAt,
                    PostsCount = c.Posts.Count,
                    SubscribersCount = c.Subscribers.Count
                })
                .ToListAsync();

            return Ok(result);
        }


        // GET: api/Communities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Community>> GetCommunity(int id)
        {
            var community = await _context.Communities.FindAsync(id);

            if (community == null)
            {
                return NotFound();
            }

            return community;
        }

        // PUT: api/Communities/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCommunity(int id, Community community)
        {
            if (id != community.Id)
            {
                return BadRequest();
            }

            _context.Entry(community).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommunityExists(id))
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

        // POST: api/Communities
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Community>> PostCommunity(CommunityDto communityDto)
        {
            var community = communityDto.CreateCommunity();
            _context.Communities.Add(community);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCommunity", new { id = community.Id }, community);
        }

        // DELETE: api/Communities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCommunity(int id)
        {
            var community = await _context.Communities.FindAsync(id);
            if (community == null)
            {
                return NotFound();
            }

            _context.Communities.Remove(community);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CommunityExists(int id)
        {
            return _context.Communities.Any(e => e.Id == id);
        }
    }
}
