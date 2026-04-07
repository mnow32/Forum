using AutoMapper;
using Forum.API.Authorization;
using Forum.API.Authorization.Constants;
using Forum.API.Boards;
using Forum.API.Data;
using Forum.API.Exceptions;
using Forum.API.Exceptions.Models;
using Forum.API.Pagination;
using Forum.API.Pagination.Params;
using Forum.API.Photos;
using Forum.API.Photos.Entities;
using Forum.API.Posts.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Forum.API.Posts.Repository
{
    public class PostsRepository(ForumDbContext dbContext, IMapper mapper, IOperationAuthorizationService authorizationService, IPhotoService photoService) : IPostsRepository
    {
        public async Task<PaginationResult<PostDto>> GetTopicPostsByIdAsync(int topicId, PagingParams pagingParams)
        {
            var topic = await dbContext.Topics.FirstOrDefaultAsync(p => p.Id == topicId);
            if(topic is null)
            {
                throw new NotFoundException($"Read failed - couldn't find Topic with id: {topicId} to retrieve Posts");
            }
            dbContext.Entry(topic).State = EntityState.Detached;

            //TODO: Possibly split this query as in https://learn.microsoft.com/en-us/ef/core/querying/single-split-queries
            var query = dbContext.Posts
                .AsQueryable()
                .OrderBy(p => p.CreatedAt)
                .Include(p => p.Replies
                    .OrderBy(r => r.CreatedAt))
                .Include(p => p.Photos)
                .AsNoTracking();

            (PaginationMetadata metadata, List<Post> posts) = await PaginationHelper.CreatePagingAsync(query, pagingParams.PageNumber, pagingParams.PageSize);

            return new PaginationResult<PostDto>
            {
                Metadata = metadata,
                Items = mapper.Map<List<PostDto>>(posts)
            };
        }

        public async Task<int> CreatePostAsync(CreatePostDto createPostDto)
        {
            var topic = await dbContext.Topics.FirstOrDefaultAsync(t => t.Id == createPostDto.TopicId);
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
            if(createPostDto.Photos is not null)
            {
                var uploadResults = await photoService.BulkUploadContentPhotosAsync(createPostDto.Photos);
                foreach (var result in uploadResults)
                {
                    if(result.Error is not null)
                    {
                        throw new CloudinaryException(result.Error.Message);
                    }
                    else
                    {
                        newPost.Photos.Add(new PostPhoto() 
                        { 
                            PublicId = result.PublicId, 
                            Url = result.SecureUrl.AbsoluteUri 
                        });
                    }
                }
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
            var post = await dbContext.Posts.Include(p => p.Photos).FirstOrDefaultAsync(p => p.Id == postId);
            if (post is null)
            {
                throw new NotFoundException($"Delete failed - couldn't find Post with id: {postId}");
            }
            bool isAuthorized = await authorizationService.IsResourceOperationAuthorizedAsync(post, ResourceOperations.Delete);
            if (!isAuthorized)
            {
                throw new ForbiddenException("Delete failed - User doesn't have permission to delete Post");
            }
            if (post.Photos is not null)
            {
                await photoService.BulkDeleteContentPhotosAsync(post.Photos.Select(photo => photo.PublicId));
                dbContext.Photos.RemoveRange(post.Photos);
            }
            dbContext.Posts.Remove(post);
            await dbContext.SaveChangesAsync();
        }


    }
}
