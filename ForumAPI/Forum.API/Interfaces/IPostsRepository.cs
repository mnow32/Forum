using Forum.API.Entities;

namespace Forum.API.Interfaces
{
    public interface IPostsRepository
    {
        Task<IEnumerable<Post>> GetAllPostsByTopicIdAsync(int topicId);
    }
}