using Forum.API.DTOs;
using Forum.API.Entities;

namespace Forum.API.Interfaces
{
    public interface IBoardsRepository
    {
        Task<IEnumerable<BoardDto>> GetAllBoardsAsync();
        Task<BoardDto> GetBoardByIdAsync(int id);
        Task<int> CreateBoardAsync(CreateBoardDto boardDto);
        Task UpdateBoardAsync(int id, UpdateBoardDto boardDto);
        Task DeleteBoardAsync(int id);
    }
}