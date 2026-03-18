using System.ComponentModel.DataAnnotations;

namespace Forum.API.Boards.DTOs
{
    public class CreateBoardDto
    {
        [StringLength(100, MinimumLength = 5)]
        public required string Name { get; set; }

        [StringLength(400, MinimumLength = 5)]
        public required string Description { get; set; }
    }
}
