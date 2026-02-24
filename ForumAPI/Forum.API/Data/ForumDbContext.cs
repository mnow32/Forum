using Forum.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Forum.API.Data
{
    internal class ForumDbContext : DbContext
    {
        internal DbSet<Post> Posts { get; set; }
        public ForumDbContext(DbContextOptions<ForumDbContext> options) : base(options)
        { 
        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Post>()
                .Property(p => p.Content)
                .IsRequired();


            modelBuilder.Entity<Post>()
                .HasOne(p => p.ParentPost)
                .WithMany(p => p.ChildrenPosts)
                .HasForeignKey(p => p.ParentPostId)
                .IsRequired(false);

            modelBuilder.Entity<Post>()
                .HasOne(p => p.ForumThread)
                .WithMany(ft => ft.Posts)
                .HasForeignKey(p => p.ThreadId)
                .IsRequired();

            modelBuilder.Entity<ForumThread>()
                .HasOne(ft => ft.Forum)
                .WithMany(f => f.ForumThreads)
                .HasForeignKey(ft => ft.ForumId)
                .IsRequired();

        }
    }
}
