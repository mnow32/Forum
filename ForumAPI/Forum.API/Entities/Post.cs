using Forum.API.Interfaces;

namespace Forum.API.Entities
{
    public class Post : IOwnable
    {
        public int Id { get; set; }
        public required string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;


        // navigation properties
        public required int TopicId { get; set; }
        public Topic Topic { get; set; } = null!;
        public List<Reply> Replies { get; set; } = new();
        public required string MemberId { get; set; }
        public ForumMember Member { get; set; } = null!;

    }
}
