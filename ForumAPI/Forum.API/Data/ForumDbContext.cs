using Forum.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Forum.API.Data
{
    public class ForumDbContext : DbContext
    {
        internal DbSet<Post> Posts { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Board> Boards { get; set; }
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

            modelBuilder.Entity<Topic>(eb =>
            {
                eb.Property(ft => ft.CreatedAt).HasDefaultValueSql("getutcdate()");
            });

            // on delete children posts must be loaded
            modelBuilder.Entity<Post>()
                .HasOne(p => p.ParentPost)
                .WithMany(p => p.ChildrenPosts)
                .HasForeignKey(p => p.ParentPostId);

            modelBuilder.Entity<Post>()
                .HasOne(p => p.Topic)
                .WithMany(ft => ft.Posts)
                .HasForeignKey(p => p.TopicId)
                .IsRequired();

            modelBuilder.Entity<Topic>()
                .HasOne(ft => ft.Board)
                .WithMany(f => f.Topics)
                .HasForeignKey(ft => ft.BoardId)
                .IsRequired();

        }
    }
}
