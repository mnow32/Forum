namespace Forum.API.Entities
{
    public class Forum
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }

        // navigation properties
        public List<ForumThread> ForumThreads { get; set; } = new List<ForumThread>();
    }
}
