using Forum.API.Entities;
using Forum.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Forum.API.Data.Repositories
{
    public class BoardsRepository(ForumDbContext dbContext) : IBoardsRepository
    {
        public async Task<IEnumerable<Board>> GetAllBoardsAsync()
        {
            var boards = await dbContext.Boards.ToListAsync();
            return boards;
        }

        public async Task<Board> GetBoardByIdAsync(int id)
        {
            var board = await dbContext.Boards
                .Include(b => b.Topics)
                .SingleOrDefaultAsync(b => b.Id == id);
            return board!;
        }
    }
}
