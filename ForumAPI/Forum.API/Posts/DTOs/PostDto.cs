using Forum.API.Replies;

namespace Forum.API.Posts.DTOs
{
    public class PostDto
    {
        public required int Id { get; set; }
        public required string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<Reply> Replies { get; set; } = new();
    }
}
