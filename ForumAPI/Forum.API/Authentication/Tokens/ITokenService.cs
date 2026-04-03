using Forum.API.Authentication.ForumUsers;

namespace Forum.API.Authentication.Tokens
{
    public interface ITokenService
    {
        Task<string> GenerateToken(ForumUser user);
        string GenerateRefreshToken();
        string HashRefreshToken(string refreshToken);
    }
}