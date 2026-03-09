namespace Forum.API.DTOs
{
    public class ForumUserDto
    {
        public required string Id { get; set; }
        public required string DisplayName { get; set; }
        public required string Email { get; set; }
        public string Token { get; set; } = null!;
    }
}
