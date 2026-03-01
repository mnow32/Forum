using AutoMapper;
using Forum.API.DTOs;
using Forum.API.Entities;
using Forum.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Forum.API.Data.Repositories
{
    public class BoardsRepository(ForumDbContext dbContext, IMapper mapper) : IBoardsRepository
    {
        public async Task<IEnumerable<BoardDto>> GetAllBoardsAsync()
        {
            var boards = await dbContext.Boards.ToListAsync();
            var boardDtos = mapper.Map<IEnumerable<Board>, IEnumerable<BoardDto>>(boards);
            return boardDtos;
        }

        public async Task<BoardDto> GetBoardByIdAsync(int id)
        {
            var board = await dbContext.Boards
                .Include(b => b.Topics)
                .FirstOrDefaultAsync(b => b.Id == id);
            if (board is null)
            {
                //TODO: Add custom exception
                throw new Exception();
            }
            var boardDto = mapper.Map<BoardDto>(board);
            return boardDto;
        }

        public async Task<int> CreateBoardAsync(CreateBoardDto boardDto)
        {
            var newBoard = mapper.Map<Board>(boardDto);
            dbContext.Boards.Add(newBoard);
            await dbContext.SaveChangesAsync();
            return newBoard.Id;
        }
        public async Task UpdateBoardAsync(int id, UpdateBoardDto boardDto)
        {
            var board = await dbContext.Boards.FindAsync(id);
            if (board is null)
            {
                //TODO: Add custom exception
                throw new Exception();
            }
            mapper.Map(boardDto, board);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteBoardAsync(int id)
        {
            var board = await dbContext.Boards.FindAsync(id);
            if(board is null)
            {
                //TODO: Add custom exception
                throw new Exception();
            }
            dbContext.Boards.Remove(board);
            await dbContext.SaveChangesAsync();
        }

    }
}
