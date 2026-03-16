using Forum.API.Constants;
using Forum.API.Entities;
using Forum.API.ForumUsers;
using Forum.API.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Forum.API.Authorization
{
    public class OperationAuthorizationService<T>(ILogger<OperationAuthorizationService<T>> logger, IForumUserContext userContext) : IOperationAuthorizationService<T> where T : IOwnable
    {
        public bool IsAuthorized(T item, string operation)
        {
            var user = userContext.GetCurrentUser();
            if(user is null)
            {
                logger.LogWarning("Couldn't authorise user to {@Operation} a resource {@ResourceId}", operation, item.Id);
                return false;
            }

            logger.LogInformation("Authorizing user {@UserEmail} to {@Operation} resource {@ResourceId}", user.Email, operation, item.Id);

            // For delete operations user must be owner or at least moderator
            if(operation == ResourceOperations.Delete 
                && (item.MemberId == user.Id || user.Roles.Contains(ForumRoles.Administrator) || user.Roles.Contains(ForumRoles.Moderator)))
            {
                logger.LogInformation("Authorization successful - user is owner or moderator");
                return true;
            }

            // For update operations user must be owner
            if(operation == ResourceOperations.Update && item.MemberId == user.Id)
            {
                logger.LogInformation("Authorization successful - user is owner of resource");
                return true;
            }

            logger.LogInformation("Authorization unsuccessful");
            return false;
        }

    }
}
