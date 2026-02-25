namespace Forum.API.Entities
{
    public class Board
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }

        // navigation properties
        public List<Topic> Topics { get; set; } = new List<Topic>();
    }
}
