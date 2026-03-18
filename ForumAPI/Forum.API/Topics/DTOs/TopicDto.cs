using Forum.API.Posts;

namespace Forum.API.Topics.DTOs
{
    public class TopicDto
    {
        public required int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<Post> Posts { get; set; } = new List<Post>();
    }
}
