using Forum.API.Interfaces;
using System.Security.Claims;

namespace Forum.API.Authorization
{
    public interface IOperationAuthorizationService
    {
        Task<bool> IsResourceOperationAuthorizedAsync<T>(T item, string operation) where T : IOwnable;
    }
}