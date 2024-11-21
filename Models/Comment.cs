using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Reddit.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public string Content { get; set; }
        public int Upvote { get; set; }
        public int Downvote { get; set; }
        public virtual Post Post { get; set; }
        public int PostId { get; set; }
        public virtual User Author { get; set; }
        public int AuthorId { get; set; }

    }
}
