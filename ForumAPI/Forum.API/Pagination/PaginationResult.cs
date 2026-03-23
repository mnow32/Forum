namespace Forum.API.Pagination
{
    public class PaginationResult<T>
    {
        public List<T> Items { get; set; } = new();
        public PaginationMetadata Metadata { get; set; } = default!;
    }
}
