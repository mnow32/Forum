using AutoMapper;
using Forum.API.Boards.DTOs;
using Forum.API.Data;
using Forum.API.Exceptions.Models;
using Forum.API.Pagination;
using Forum.API.Pagination.Params;
using Microsoft.EntityFrameworkCore;

namespace Forum.API.Boards.Repository
{
    public class BoardsRepository(ForumDbContext dbContext, IMapper mapper) : IBoardsRepository
    {
        public async Task<PaginationResult<BoardDto>> GetBoardsAsync(BoardParams boardParams)
        {
            var query = dbContext.Boards.AsQueryable().AsNoTracking();

            if(boardParams.Name.Length > 0)
            {
                query = query.Where(b => b.Name.Contains(boardParams.Name));
            }

            var result = await PaginationHelper.CreatePagingAsync(query, boardParams.PageNumber, boardParams.PageSize);

            return new PaginationResult<BoardDto>
            {
                Metadata = result.Metadata,
                Items = mapper.Map<List<BoardDto>>(result.Items)
            };
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
