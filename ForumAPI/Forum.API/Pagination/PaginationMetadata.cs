namespace Forum.API.Pagination
{
    public class PaginationMetadata(int currentPage, int pageSize, int pagesCount, int itemsCount)
    {
        public int CurrentPage { get; set; } = currentPage;
        public int PageSize { get; set; } = pageSize;
        public int PagesCount { get; set; } = pagesCount;
        public int ItemsCount { get; set; } = itemsCount;
    }
}
