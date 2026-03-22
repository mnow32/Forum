using System.ComponentModel.DataAnnotations;

namespace Forum.API.Topics.DTOs
{
    public class CreateTopicDto
    {
        public int BoardId { get; set; }
        public string MemberId { get; set; } = string.Empty;
        public string MemberName { get; set; } = string.Empty;

        [StringLength(250, MinimumLength = 5)]
        public required string Title { get; set; }

        [StringLength(30000, MinimumLength = 5)]
        public required string Description { get; set; }
    }
}
