using System.ComponentModel.DataAnnotations;

namespace Forum.API.Posts.DTOs
{
    public class CreatePostDto
    {
        public int TopicId { get; set; }
        public string? MemberId { get; set; }

        [StringLength(30000, MinimumLength = 5)]
        public required string Content { get; set; }
    }
}
