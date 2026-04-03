namespace Forum.API.Photos.Entities
{
    public class Photo
    {
        public int Id { get; set; }
        public required string Url { get; set; }
        public required string PublicId { get; set; }
    }
}
