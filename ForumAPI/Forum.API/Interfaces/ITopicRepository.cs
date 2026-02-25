using Forum.API.Entities;

namespace Forum.API.Interfaces
{
    public interface ITopicRepository
    {
        Task<Topic> GetTopicByIdAsync(int id);
    }
}