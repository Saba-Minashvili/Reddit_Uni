using Reddit.Models;

namespace Reddit.Dtos
{
    public class CommunityDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int OwnerId { get; set; }

        public Community CreateCommunity()
        {
            return new Community() { Name = Name, Description = Description, OwnerId = OwnerId };
        }
    }
}
