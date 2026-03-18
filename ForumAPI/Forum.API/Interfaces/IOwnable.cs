namespace Forum.API.Interfaces
{
    public interface IOwnable
    {
        public int Id { get; set; }
        string MemberId { get; set; }
    }
}
