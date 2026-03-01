using Forum.API.Entities;

namespace Forum.API.DTOs
{
    public class BoardDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public List<TopicDto> Topics { get; set; } = new List<TopicDto>();
    }
}
