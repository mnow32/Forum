using Forum.API.Interfaces;

namespace Forum.API.Authorization
{
    public interface IOperationAuthorizationService<T> where T : IOwnable
    {
        bool IsAuthorized(T item, string operation);
    }
}