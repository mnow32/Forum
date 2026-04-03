using Forum.API.Photos.Entities;

namespace Forum.API.ForumMembers.DTOs
{
    public class ForumMemberDto
    {
        public string? Id { get; set; } 
        public string? DisplayName { get; set; } 
        public string? Gender { get; set; } 
        public string? Country { get; set; } 
        public string? Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastActive { get; set; }
        public MemberPhoto? Photo { get; set; }
    }
}
