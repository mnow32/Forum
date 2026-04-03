using Forum.API.Authentication.ForumUsers;
using Forum.API.Boards;
using Forum.API.ForumMembers;
using Forum.API.Photos.Entities;
using Forum.API.Posts;
using Forum.API.Replies;
using Forum.API.Topics;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Forum.API.Data
{
    public class ForumDbContext(DbContextOptions<ForumDbContext> options) : IdentityDbContext<ForumUser>(options)
    {
        public DbSet<ForumMember> Members { get; set; }
        public DbSet<Board> Boards { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Reply> Replies { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<MemberPhoto> MemberPhotos { get; set; }
        public DbSet<TopicPhoto> TopicPhotos { get; set; }
        public DbSet<PostPhoto> PostPhotos { get; set; }
        public DbSet<ReplyPhoto> ReplyPhotos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<ForumMember>(eb =>
            {
                eb.Property(fm => fm.CreatedAt).HasDefaultValueSql("getutcdate()");
            });

            modelBuilder.Entity<Post>(eb =>
            {
                eb.Property(p => p.CreatedAt).HasDefaultValueSql("getutcdate()");
            });

            modelBuilder.Entity<Topic>(eb =>
            {
                eb.Property(t => t.CreatedAt).HasDefaultValueSql("getutcdate()");
            });

            modelBuilder.Entity<Reply>(eb =>
            {
                eb.Property(r => r.CreatedAt).HasDefaultValueSql("getutcdate()");
            });

            modelBuilder.Entity<ForumMember>()
                .HasOne(fm => fm.User)
                .WithOne(u => u.Member)
                .HasForeignKey<ForumMember>(fm => fm.Id)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ForumMember>()
                .HasOne(fm => fm.Photo)
                .WithOne(mp  => mp.Member)
                .HasForeignKey<MemberPhoto>(mp => mp.MemberId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reply>()
                .HasOne(r => r.Post)
                .WithMany(p => p.Replies)
                .HasForeignKey(r => r.PostId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reply>()
                .HasOne(r => r.Member)
                .WithMany(m => m.Replies)
                .HasForeignKey(r => r.MemberId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reply>()
                .HasMany(r => r.Photos)
                .WithOne(rp => rp.Reply)
                .HasForeignKey(rp => rp.ReplyId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Post>()
                .HasOne(p => p.Topic)
                .WithMany(t => t.Posts)
                .HasForeignKey(p => p.TopicId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Post>()
                .HasOne(p => p.Member)
                .WithMany(m => m.Posts)
                .HasForeignKey(p => p.MemberId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Post>()
                .HasMany(p => p.Photos)
                .WithOne(pp => pp.Post)
                .HasForeignKey(rp => rp.PostId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Topic>()
                .HasOne(t => t.Board)
                .WithMany(b => b.Topics)
                .HasForeignKey(t => t.BoardId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Topic>()
                .HasOne(t => t.Member)
                .WithMany(m => m.Topics)
                .HasForeignKey(t => t.MemberId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Topic>()
                .HasMany(t => t.Photos)
                .WithOne(tp => tp.Topic)
                .HasForeignKey(tp => tp.TopicId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
