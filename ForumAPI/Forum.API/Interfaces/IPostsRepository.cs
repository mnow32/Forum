using Forum.API.Posts;

namespace Forum.API.Interfaces
{
    public interface IPostsRepository
    {
        Task<IEnumerable<Post>> GetAllPostsByTopicIdAsync(int topicId);
    }
}