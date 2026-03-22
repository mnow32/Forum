using Forum.API.Topics.DTOs;

namespace Forum.API.Boards.DTOs
{
    public class BoardDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<TopicDto> Topics { get; set; } = new List<TopicDto>();
    }
}
