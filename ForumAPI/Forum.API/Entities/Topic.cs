using Forum.API.Interfaces;

namespace Forum.API.Entities
{
    public class Topic : IOwnable
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;


        // navigation properties
        public required int BoardId { get; set; }
        public Board Board { get; set; } = null!;
        public List<Post> Posts { get; set; } = new List<Post>();
        public required string MemberId { get; set; }
        public ForumMember Member { get; set; } = null!;


    }
}
