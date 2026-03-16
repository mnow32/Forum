using System.Security.Claims;

namespace Forum.API.ForumUsers
{
    public class ForumUserContext(IHttpContextAccessor httpContextAccessor) : IForumUserContext
    {
        public CurrentUser? GetCurrentUser()
        {
            ClaimsPrincipal? user = httpContextAccessor.HttpContext?.User;

            if (user is null)
            {
                throw new InvalidOperationException("User context is not present");
            }

            if (user.Identity is null || !user.Identity.IsAuthenticated)
            {
                return null;
            }

            string userId = user.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            string userEmail = user.FindFirst(ClaimTypes.Email)!.Value;
            IEnumerable<string> userRoles = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(r => r.Value);

            return new CurrentUser(userId, userEmail, userRoles);
        }
    }
}
