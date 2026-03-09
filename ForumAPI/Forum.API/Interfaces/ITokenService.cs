using Forum.API.Entities;

namespace Forum.API.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(ForumUser user);
    }
}