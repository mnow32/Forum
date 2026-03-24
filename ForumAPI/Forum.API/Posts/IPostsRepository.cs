using Forum.API.Pagination;
using Forum.API.Pagination.Params;
using Forum.API.Posts.DTOs;

namespace Forum.API.Posts
{
    public interface IPostsRepository
    {
        Task<PostDto> GetPostByIdAsync(int id);
        Task<PaginationResult<PostDto>> GetTopicPostsByIdAsync(int topicId, PagingParams pagingParams);
        Task<int> CreatePostAsync(CreatePostDto createPostDto);
        Task UpdatePostAsync(int postId, UpdatePostDto updatePostDto);
        Task DeletePostAsync(int postId);
    }
}