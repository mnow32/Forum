using Forum.API.ForumMembers;
using Microsoft.AspNetCore.Identity;

namespace Forum.API.ForumUsers
{
    public class ForumUser : IdentityUser
    {
        public required string DisplayName { get; set; }
        public string? RefreshTokenHash { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }

        // navigation properties
        public ForumMember Member { get; set; } = null!;
    }
}
