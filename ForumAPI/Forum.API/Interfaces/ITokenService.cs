using Forum.API.ForumUsers;

namespace Forum.API.Interfaces
{
    public interface ITokenService
    {
        Task<string> GenerateToken(ForumUser user);
        string GenerateRefreshToken();
        string HashRefreshToken(string refreshToken);
    }
}