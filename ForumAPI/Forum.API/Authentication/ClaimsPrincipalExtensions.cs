using Forum.API.Exceptions.Models;
using System.Security.Claims;

namespace Forum.API.Authentication
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetMemberId(this ClaimsPrincipal user)
        {
            string userId = user.FindFirst(ClaimTypes.NameIdentifier)!.Value 
                ?? throw new TokenException("Cannot retrieve MemberId from the token");
            return userId;
        }

        public static string GetMemberName(this ClaimsPrincipal user)
        {
            string userName = user.FindFirst(ClaimTypes.Name)!.Value
                ?? throw new TokenException("Cannot retrieve MemberName from the token");
            return userName;
        }
    }
}
