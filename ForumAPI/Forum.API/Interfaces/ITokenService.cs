using Forum.API.Entities;

namespace Forum.API.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(ForumUser user);
        string GenerateRefreshToken();
        string HashRefreshToken(string refreshToken);
    }
}