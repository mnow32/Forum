using Forum.API.ForumUsers;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Forum.API.Tokens
{
    public class TokenService(IConfiguration config, UserManager<ForumUser> userManager) : ITokenService
    {
        public async Task<string> GenerateToken(ForumUser user)
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
                new(ClaimTypes.Name, user.DisplayName),
                new(ClaimTypes.Email, user.Email!),
            };

            var userRoles = await userManager.GetRolesAsync(user);

            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

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

        public string GenerateRefreshToken()
        {
            var randomBytes = RandomNumberGenerator.GetBytes(64);
            return Convert.ToBase64String(randomBytes);
        }

        public string HashRefreshToken(string refreshToken)
        {
            using SHA256 sha = SHA256.Create();
            var hashedToken = sha.ComputeHash(Encoding.UTF8.GetBytes(refreshToken));
            return Convert.ToBase64String(hashedToken);
        }
    }
}
