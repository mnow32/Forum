namespace Forum.API.Replies.DTOs
{
    public class ReplyDto
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public string MemberName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; } 
    }
}
