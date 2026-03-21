using System.ComponentModel.DataAnnotations;

namespace Forum.API.Replies.DTOs
{
    public class UpdateReplyDto
    {
        [StringLength(30000, MinimumLength = 5)]
        public required string Content { get; set; }
    }
}
