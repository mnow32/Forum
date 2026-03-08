namespace Forum.API.Entities
{
    public class ForumMember
    {
        public required string Id { get; set; }
        public required string DisplayName { get; set; }
        public string? Gender { get; set; }
        public string? Country { get; set; }
        public string? Descripiton { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastActive { get; set; } = DateTime.UtcNow;


        // Mavigation properties
        public ForumUser User { get; set; } = null!;
        public List<Reply> Replies { get; set; } = new();
        public List<Post> Posts { get; set; } = new();
        public List<Topic> Topics { get; set; } = new();
    }
}
