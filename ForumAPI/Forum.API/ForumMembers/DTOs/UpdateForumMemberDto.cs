using System.Text.Json.Serialization;

namespace Forum.API.ForumMembers.DTOs
{
    public class UpdateForumMemberDto
    {
        public string? Id { get; set; }
        public string? DisplayName { get; set; } 
        public string? Gender { get; set; }
        public string? Country { get; set; }
        public string? Description { get; set; } 
        public IFormFile? Photo { get; set; }

    }
}
