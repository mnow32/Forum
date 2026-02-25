using Forum.API.Entities;

namespace Forum.API.Interfaces
{
    public interface ITopicsRepository
    {
        Task<Topic> GetTopicByIdAsync(int id);
    }
}