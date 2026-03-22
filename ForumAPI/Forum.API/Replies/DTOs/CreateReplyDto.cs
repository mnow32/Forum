using System.ComponentModel.DataAnnotations;

namespace Forum.API.Replies.DTOs
{
    public class CreateReplyDto
    {
        public int PostId { get; set; }
        public string MemberId { get; set; } = string.Empty;
        public string MemberName { get; set; } = string.Empty;

        [StringLength(30000, MinimumLength = 5)]
        public required string Content { get; set; }
    }
}
