using Forum.API.Entities;

namespace Forum.API.Interfaces
{
    public interface IBoardsRepository
    {
        Task<IEnumerable<Board>> GetAllBoardsAsync();
        Task<Board> GetBoardByIdAsync(int id);
        Task<int> CreateAsync(Board board);
    }
}