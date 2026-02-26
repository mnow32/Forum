namespace Forum.API.DTOs
{
    public class CreateBoardDto
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
    }
}
