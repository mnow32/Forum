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

            modelBuilder.Entity<Post>(eb =>
            {
                eb.Property(p => p.CreatedAt).HasDefaultValueSql("getutcdate()");
            });

            modelBuilder.Entity<ForumThread>(eb =>
            {
                eb.Property(ft => ft.CreatedAt).HasDefaultValueSql("getutcdate()");
            });

            // on delete children posts must be loaded
            modelBuilder.Entity<Post>()
                .HasOne(p => p.ParentPost)
                .WithMany(p => p.ChildrenPosts)
                .HasForeignKey(p => p.ParentPostId)
                .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<Post>()
                .HasOne(p => p.ForumThread)
                .WithMany(ft => ft.Posts)
                .HasForeignKey(p => p.ForumThreadId)
                .IsRequired();

            modelBuilder.Entity<ForumThread>()
                .HasOne(ft => ft.Forum)
                .WithMany(f => f.ForumThreads)
                .HasForeignKey(ft => ft.ForumId)
                .IsRequired();

        }
    }
}
