using Forum.API.Posts;
using Forum.API.Posts.DTOs;

namespace Forum.API.Interfaces
{
    public interface IPostsRepository
    {
        Task<PostDto> GetPostByIdAsync(int id);
        Task<int> CreatePostAsync(CreatePostDto createPostDto);
        Task UpdatePostAsync(int postId, UpdatePostDto updatePostDto);
        Task DeletePostAsync(int postId);
    }
}