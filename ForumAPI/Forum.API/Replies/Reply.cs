using Forum.API.ForumMembers;
using Forum.API.Interfaces;
using Forum.API.Posts;

namespace Forum.API.Replies
{
    public class Reply : IOwnable
    {
        public int Id { get; set; }
        public required string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public required int PostId { get; set; }
        public Post Post { get; set; } = null!;
        public required string MemberId { get; set; }
        public ForumMember Member { get; set; } = null!;
    }
}
