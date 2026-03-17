using Forum.API.Exceptions;
using System.Security.Claims;

namespace Forum.API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetMemberId(this ClaimsPrincipal user)
        {
            string userId = user.FindFirst(ClaimTypes.NameIdentifier)!.Value 
                ?? throw new TokenException("Cannot retrieve MemberId from the token");
            return userId;
        }

        public static IEnumerable<string> GetMemberRoles(this ClaimsPrincipal user)
        {
            IEnumerable<string> userRoles = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(r => r.Value)
                ?? throw new TokenException("Cannot retrieve Member roles from the token");
            return userRoles;
        }
    }
}
