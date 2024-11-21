using Reddit.Models;

namespace Reddit.Repositories
{
    public interface IPostsRepository
    {
        public Task<PagedList<Post>> GetPosts(int pageNumber, int pageSize, string? searchTerm, string? sortTerm = null, bool isAscending = true);

    }
}