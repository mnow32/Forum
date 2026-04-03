using Forum.API.Photos.Entities;
using Forum.API.Replies.DTOs;

namespace Forum.API.Posts.DTOs
{
    public class PostDto
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public string MemberName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<ReplyDto> Replies { get; set; } = new();
        public List<PostPhoto> Photos { get; set; } = new();
    }
}
