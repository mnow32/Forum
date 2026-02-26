namespace Forum.API.DTOs
{
    public class CreateTopicDto
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
    }
}
