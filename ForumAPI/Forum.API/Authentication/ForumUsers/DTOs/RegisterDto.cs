using System.ComponentModel.DataAnnotations;

namespace Forum.API.Authentication.ForumUsers.DTOs
{
    public class RegisterDto
    {
        [Required]
        [StringLength(50)]
        public required string DisplayName { get; set; }

        [EmailAddress]
        [Required]
        public required string Email { get; set; }

        [StringLength(50, MinimumLength = 8)]
        public required string Password { get; set; }
    }
}
