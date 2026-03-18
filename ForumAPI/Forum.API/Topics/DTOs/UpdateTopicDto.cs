using System.ComponentModel.DataAnnotations;

namespace Forum.API.Topics.DTOs
{
    public class UpdateTopicDto
    {
        [StringLength(250, MinimumLength = 5)]
        public string? Title { get; set; }

        [StringLength(30000, MinimumLength = 5)]
        public string? Description { get; set; }
    }
}
