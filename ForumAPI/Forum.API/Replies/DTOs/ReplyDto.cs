namespace Forum.API.Replies.DTOs
{
    public class ReplyDto
    {
        public int Id { get; set; }
        public required string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; } 
    }
}
