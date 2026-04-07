using Forum.API.Boards.DTOs;
using Forum.API.Pagination;
using Forum.API.Pagination.Params;

namespace Forum.API.Boards.Repository
{
    public interface IBoardsRepository
    {
        Task<PaginationResult<BoardDto>> GetBoardsAsync(BoardParams boardParams);
        Task<int> CreateBoardAsync(CreateBoardDto boardDto);
        Task UpdateBoardAsync(int id, UpdateBoardDto boardDto);
        Task DeleteBoardAsync(int id);
    }
}