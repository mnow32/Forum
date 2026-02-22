using Microsoft.EntityFrameworkCore;

namespace Forum.API.Data
{
    public class ForumDbContext : DbContext
    {
        public ForumDbContext(DbContextOptions<ForumDbContext> options) : base(options)
        {
            
        }
    }
}
