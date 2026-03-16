using System.ComponentModel.DataAnnotations;

namespace Forum.API.ForumUsers.DTOs
{
    public class LoginDto
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
    }
}
