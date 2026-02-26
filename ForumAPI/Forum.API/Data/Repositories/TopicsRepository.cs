using Forum.API.Entities;
using Forum.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Forum.API.Data.Repositories
{
    public class TopicsRepository(ForumDbContext dbContext) : ITopicsRepository
    {
        public async Task<Topic> GetTopicByIdAsync(int id)
        {
            var topic = await dbContext.Topics
                .Include(t => t.Posts)
                .SingleOrDefaultAsync(t => t.Id == id);
            return topic!;
        }
    }
}
