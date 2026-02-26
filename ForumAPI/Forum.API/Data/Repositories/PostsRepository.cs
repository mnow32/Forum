using Forum.API.Entities;
using Forum.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Forum.API.Data.Repositories
{
    public class PostsRepository(ForumDbContext dbContext) : IPostsRepository
    {
        public async Task<IEnumerable<Post>> GetAllPostsByTopicIdAsync(int topicId)
        {
            var posts = await dbContext.Posts
                .Where(p => p.TopicId == topicId)
                .ToListAsync();

            return posts;
        }
    }
}
