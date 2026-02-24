namespace Forum.API.Entities
{
    public class ForumThread
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required DateTime CreatedAt { get; set; }

        // navigation properties
        public required int ForumId { get; set; }
        public Forum Forum { get; set; } = null!;
        public List<Post> Posts { get; set; } = new List<Post>();

    }
}
