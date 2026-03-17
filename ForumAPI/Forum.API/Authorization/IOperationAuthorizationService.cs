using Forum.API.Interfaces;
using System.Security.Claims;

namespace Forum.API.Authorization
{
    public interface IOperationAuthorizationService
    {
        bool IsResourceOperationAuthorized<T>(T item, string operation, ClaimsPrincipal user) where T : IOwnable;
    }
}