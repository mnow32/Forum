using Microsoft.AspNetCore.Identity;

namespace Forum.API.Entities
{
    public class ForumUser : IdentityUser
    {
        public required string DisplayName { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }
        // navigation properties
        public ForumMember Member { get; set; } = null!;
    }
}
