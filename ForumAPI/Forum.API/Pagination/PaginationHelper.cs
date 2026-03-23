using Microsoft.EntityFrameworkCore;

namespace Forum.API.Pagination
{
    public static class PaginationHelper
    {
        public static async Task<PaginationResult<T>> CreatePagingAsync<T>(IQueryable<T> query, int pageNumber, int pageSize)
        {
            int count = await query.CountAsync();
            List<T> items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginationResult<T>
            {
                Metadata = new PaginationMetadata(
                    currentPage: pageNumber,
                    pageSize: pageSize,
                    pagesCount: (int)Math.Ceiling(count / (double)pageSize),
                    itemsCount: count),
                Items = items
            };
        }
    }
}
