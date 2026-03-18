using Forum.API.Topics.DTOs;

namespace Forum.API.Boards.DTOs
{
    public class BoardDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public List<TopicDto> Topics { get; set; } = new List<TopicDto>();
    }
}
