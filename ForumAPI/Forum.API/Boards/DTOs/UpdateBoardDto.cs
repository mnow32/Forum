using System.ComponentModel.DataAnnotations;

namespace Forum.API.Boards.DTOs
{
    public class UpdateBoardDto
    {
        [StringLength(100, MinimumLength = 5)]
        public string? Name { get; set; }

        [StringLength(400, MinimumLength = 5)]
        public string? Description { get; set; }
    }
}
