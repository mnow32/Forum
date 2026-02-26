using Forum.API.DTOs;
using Forum.API.Entities;

namespace Forum.API.Extensions
{
    public static class BoardExtensions
    {
        public static Board FromDto(CreateBoardDto dto)
        {
            return new Board
            {
                Name = dto.Name,
                Description = dto.Description
            };
        }
    }
}
