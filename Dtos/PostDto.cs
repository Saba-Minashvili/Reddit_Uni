using Reddit.Models;

namespace Reddit.Dtos
{
    public class PostDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public int AuthorId { get; set; }
        public int CommunityId { get; set; }

        public Post CreatePost() {
        return new Post { Title = Title, Content = Content,
            AuthorId = AuthorId,
            CommunityId = CommunityId
        };
        }
    }
}
