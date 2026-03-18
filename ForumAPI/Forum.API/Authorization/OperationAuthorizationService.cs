using Forum.API.Authorization.Constants;
using Forum.API.Extensions;
using Forum.API.Interfaces;
using System.Security.Claims;

namespace Forum.API.Authorization
{
    public class OperationAuthorizationService(ILogger<OperationAuthorizationService> logger) : IOperationAuthorizationService
    {
        
        public bool IsResourceOperationAuthorized<T>(T item, string operation, ClaimsPrincipal user) where T : IOwnable
        {
            if (user is null)
            {
                logger.LogWarning("Couldn't authorise user to {@Operation} a resource {@ResourceId}", operation, item.Id);
                return false;
            }

            string userId = user.GetMemberId();

            logger.LogInformation("Authorizing user to {@Operation} resource {@ResourceId}", operation, item.Id);

            // For delete operations user must be owner or at least moderator
            if (operation == ResourceOperations.Delete
                && (item.MemberId == userId || user.IsInRole(ForumRoles.Administrator) || user.IsInRole(ForumRoles.Moderator)))
            {
                logger.LogInformation("Authorization successful - user is owner or moderator");
                return true;
            }

            // For update operations user must be owner
            if (operation == ResourceOperations.Update && item.MemberId == userId)
            {
                logger.LogInformation("Authorization successful - user is owner of resource");
                return true;
            }

            logger.LogInformation("Authorization unsuccessful");
            return false;
        }

    }
}
