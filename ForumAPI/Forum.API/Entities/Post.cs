namespace Forum.API.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public required string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }


        // navigation properties
        public int? ParentPostId { get; set; }
        public Post? ParentPost { get; set; }
        public List<Post> ChildrenPosts { get; set; } = new List<Post>();

        public required int ForumThreadId { get; set; }
        public ForumThread ForumThread { get; set; } = null!;


    }
}
