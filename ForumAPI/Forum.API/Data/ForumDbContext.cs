using Forum.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Forum.API.Data
{
    public class ForumDbContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }
        public ForumDbContext(DbContextOptions<ForumDbContext> options) : base(options)
        { 
        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }
    }
}
