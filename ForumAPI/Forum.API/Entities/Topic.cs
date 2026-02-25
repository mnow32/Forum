namespace Forum.API.Entities
{
    public class Topic
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }


        // navigation properties
        public required int BoardId { get; set; }
        public Board Board { get; set; } = null!;
        public List<Post> Posts { get; set; } = new List<Post>();

    }
}
