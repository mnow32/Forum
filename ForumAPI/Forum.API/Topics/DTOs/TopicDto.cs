using Forum.API.Posts.DTOs;

namespace Forum.API.Topics.DTOs
{
    public class TopicDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string MemberName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<PostDto> Posts { get; set; } = new();
    }
}
