using Forum.API.Authentication;
using Forum.API.Authorization.Constants;
using Forum.API.Data;
using Forum.API.Exceptions.Models;
using Forum.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Forum.API.Authorization
{
    public class OperationAuthorizationService(ILogger<OperationAuthorizationService> logger, 
        IHttpContextAccessor httpContextAccessor, 
        ForumDbContext dbContext) : IOperationAuthorizationService
    {
        
        public async Task<bool> IsResourceOperationAuthorizedAsync<T>(T item, string operation) where T : IOwnable
        {            
            logger.LogInformation("Authorizing user to @{Operation} a @{ResourceName}.", operation, item.GetType().Name);
            var user = httpContextAccessor.HttpContext!.Request.HttpContext.User;
            if (user is null)
            {
                logger.LogWarning("Authorization failed - couldn't find user in HTTP context.");
                return false;
            }

            string memberId = user.GetMemberId();

            var member = await dbContext.Members.AsNoTracking().FirstOrDefaultAsync(m => m.Id == memberId);

            if(member is null)
            {
                throw new NotFoundException("Authorization failed - couldn't find corresponding member in database.");
            }

            // For create operation user must only be autenticated and exist in database
            if (operation == ResourceOperations.Create)
            {
                logger.LogInformation("Authorization successful - creating resource");
                return true;
            }

            // For delete operations user must be owner or at least moderator
            if (operation == ResourceOperations.Delete
                && (item.MemberId == memberId || user.IsInRole(ForumRoles.Administrator) || user.IsInRole(ForumRoles.Moderator)))
            {
                logger.LogInformation("Authorization successful - user is owner or moderator");
                return true;
            }

            // For update operations user must be owner
            if (operation == ResourceOperations.Update && item.MemberId == memberId)
            {
                logger.LogInformation("Authorization successful - user is owner of resource");
                return true;
            }

            logger.LogInformation("Authorization unsuccessful");
            return false;
        }

    }
}
