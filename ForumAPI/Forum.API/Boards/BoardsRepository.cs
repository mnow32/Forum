using AutoMapper;
using Forum.API.Boards.DTOs;
using Forum.API.Data;
using Forum.API.Exceptions;
using Forum.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Forum.API.Boards
{
    public class BoardsRepository(ForumDbContext dbContext, IMapper mapper) : IBoardsRepository
    {
        public async Task<IEnumerable<BoardDto>> GetAllBoardsAsync()
        {
            var boards = await dbContext.Boards.AsNoTracking().ToListAsync();
            var boardDtos = mapper.Map<IEnumerable<BoardDto>>(boards);
            return boardDtos;
        }

        public async Task<BoardDto> GetBoardByIdAsync(int id)
        {
            var board = await dbContext.Boards
                .Include(b => b.Topics)
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == id);
            if (board is null)
            {
                throw new NotFoundException($"Read failed - couldn't find Board with id: {id}");
            }
            var boardDto = mapper.Map<BoardDto>(board);
            return boardDto;
        }

        public async Task<int> CreateBoardAsync(CreateBoardDto boardDto)
        {
            var newBoard = mapper.Map<Board>(boardDto);
            var result = dbContext.Boards.Add(newBoard);
            await dbContext.SaveChangesAsync();
            return newBoard.Id;
        }
        public async Task UpdateBoardAsync(int id, UpdateBoardDto boardDto)
        {
            var board = await dbContext.Boards.FirstOrDefaultAsync(b => b.Id == id);
            if (board is null)
            {
                throw new NotFoundException($"Update failed - couldn't find Board with id: {id}");
            }            
            mapper.Map(boardDto, board);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteBoardAsync(int id)
        {
            var board = await dbContext.Boards.FirstOrDefaultAsync(b => b.Id == id);
            if (board is null)
            {
                throw new NotFoundException($"Delete failed - couldn't find Board with id: {id}");
            }
            dbContext.Boards.Remove(board);
            await dbContext.SaveChangesAsync();
        }

    }
}
