using Forum.API.Entities;
using Forum.API.Interfaces;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Forum.API.Services
{
    public class TokenService(IConfiguration config) : ITokenService
    {
        public string GenerateToken(ForumUser user)
        {
            var tokenKey = config.GetValue<string>("Forum:TokenKey");
            if (tokenKey is null)
            {
                //TODO: Add logging and custom exception
                throw new Exception("Cannot get token key");
            }
            if (tokenKey.Length < 64)
            {
                //TODO: Add logging and custom exception
                throw new Exception("Token key has tobe at least 64 characters");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var claims = new List<Claim>()
            {
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Email, user.Email!),
            };

            var claimsIdentity = new ClaimsIdentity(claims);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddMinutes(8),
                SigningCredentials = credentials
            };

            var tokenHandler = new JsonWebTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return token;
        }
    }
}
