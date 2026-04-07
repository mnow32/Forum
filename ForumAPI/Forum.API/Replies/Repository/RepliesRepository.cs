using AutoMapper;
using Forum.API.Authorization;
using Forum.API.Authorization.Constants;
using Forum.API.Data;
using Forum.API.Exceptions;
using Forum.API.Exceptions.Models;
using Forum.API.Photos;
using Forum.API.Photos.Entities;
using Forum.API.Replies.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Forum.API.Replies.Repository
{
    public class RepliesRepository(ForumDbContext dbContext, IMapper mapper, IOperationAuthorizationService authorizationService, IPhotoService photoService) : IRepliesRepository
    {
        public async Task<int> CreateReplyAsync(CreateReplyDto createReplyDto)
        {
            var post = dbContext.Posts.FirstOrDefaultAsync(p => p.Id == createReplyDto.PostId);
            if (post is null)
            {
                throw new NotFoundException($"Create failed - couldn't find Post with id: {createReplyDto.PostId} to create Reply");
            }
            Reply newReply = mapper.Map<Reply>(createReplyDto);
            bool isAuthorized = await authorizationService.IsResourceOperationAuthorizedAsync(newReply, ResourceOperations.Create);
            if (!isAuthorized)
            {
                throw new ForbiddenException("Create failed - User doesn't have permission to create Post");
            }
            if (createReplyDto.Photos is not null)
            {
                var uploadResults = await photoService.BulkUploadContentPhotosAsync(createReplyDto.Photos);
                foreach (var result in uploadResults)
                {
                    if (result.Error is not null)
                    {
                        throw new CloudinaryException(result.Error.Message);
                    }
                    else
                    {
                        newReply.Photos.Add(new ReplyPhoto()
                        {
                            PublicId = result.PublicId,
                            Url = result.SecureUrl.AbsoluteUri
                        });
                    }
                }
            }
            dbContext.Replies.Add(newReply);
            await dbContext.SaveChangesAsync();

            return newReply.Id;
        }

        public async Task UpdateReplyAsync(int replyId, UpdateReplyDto updateReplyDto)
        {
            var reply = await dbContext.Replies.FirstOrDefaultAsync(r => r.Id == replyId);
            if (reply is null)
            {
                throw new NotFoundException($"Update failed - couldn't find Reply with id: {replyId}");
            }
            bool isAuthorized = await authorizationService.IsResourceOperationAuthorizedAsync(reply, ResourceOperations.Update);
            if (!isAuthorized)
            {
                throw new ForbiddenException("Update failed - User doesn't have permission to update Reply");
            }
            var newReply = mapper.Map(updateReplyDto, reply);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteReplyAsync(int replyId)
        {
            var reply = await dbContext.Replies.Include(r => r.Photos).FirstOrDefaultAsync(r => r.Id == replyId);
            if (reply is null)
            {
                throw new NotFoundException($"Delete failed - couldn't find Reply with id: {replyId}");
            }
            bool isAuthorized = await authorizationService.IsResourceOperationAuthorizedAsync(reply, ResourceOperations.Delete);
            if (!isAuthorized)
            {
                throw new ForbiddenException("Delete failed - User doesn't have permission to delete Post");
            }
            if (reply.Photos is not null)
            {
                await photoService.BulkDeleteContentPhotosAsync(reply.Photos.Select(photo => photo.PublicId));
                dbContext.Photos.RemoveRange(reply.Photos);
            }
            dbContext.Replies.Remove(reply);
            await dbContext.SaveChangesAsync();
        }
    }
}
