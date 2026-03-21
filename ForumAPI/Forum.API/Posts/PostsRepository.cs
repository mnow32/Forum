using AutoMapper;
using Forum.API.Authorization;
using Forum.API.Authorization.Constants;
using Forum.API.Data;
using Forum.API.Exceptions;
using Forum.API.Interfaces;
using Forum.API.Posts.DTOs;
using Forum.API.Topics.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Forum.API.Posts
{
    public class PostsRepository(ForumDbContext dbContext, IMapper mapper, IOperationAuthorizationService authorizationService) : IPostsRepository
    {
        public async Task<PostDto> GetPostByIdAsync(int id)
        {
            var post = await dbContext.Posts
                .Include(p => p.Replies)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
            if(post is null)
            {
                throw new NotFoundException($"Read failed - couldn't find Post with id: {id}");
            }

            PostDto postDto = mapper.Map<PostDto>(post);
            return postDto;
        }

        public async Task<int> CreatePostAsync(CreatePostDto createPostDto)
        {
            var topic = dbContext.Topics.FirstOrDefaultAsync(t => t.Id == createPostDto.TopicId);
            if(topic is null)
            {
                throw new NotFoundException($"Create failed - couldn't find Topic with id: {createPostDto.TopicId} to create Post");
            }
            Post newPost = mapper.Map<Post>(createPostDto);
            bool isAuthorized = await authorizationService.IsResourceOperationAuthorizedAsync(newPost, ResourceOperations.Create);
            if(!isAuthorized)
            {
                throw new ForbiddenException("Create failed - User doesn't have permission to create Post");
            }

            dbContext.Posts.Add(newPost);
            await dbContext.SaveChangesAsync();

            return newPost.Id;
        }

        public async Task UpdatePostAsync(int postId, UpdatePostDto updatePostDto)
        {
            var post = await dbContext.Posts.FirstOrDefaultAsync(p => p.Id == postId);
            if (post is null)
            {
                throw new NotFoundException($"Update failed - couldn't find Post with id: {postId}");
            }
            bool isAuthorized = await authorizationService.IsResourceOperationAuthorizedAsync(post, ResourceOperations.Update);
            if (!isAuthorized)
            {
                throw new ForbiddenException("Update failed - User doesn't have permission to update Topic");
            }
            var newTopic = mapper.Map(updatePostDto, post);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeletePostAsync(int postId)
        {
            var post = await dbContext.Posts.FirstOrDefaultAsync(p => p.Id == postId);
            if (post is null)
            {
                throw new NotFoundException($"Delete failed - couldn't find Post with id: {postId}");
            }
            bool isAuthorized = await authorizationService.IsResourceOperationAuthorizedAsync(post, ResourceOperations.Delete);
            if (!isAuthorized)
            {
                throw new ForbiddenException("Delete failed - User doesn't have permission to delete Post");
            }
            dbContext.Posts.Remove(post);
            await dbContext.SaveChangesAsync();
        }


    }
}
