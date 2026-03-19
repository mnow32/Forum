using AutoMapper;
using Forum.API.Authorization;
using Forum.API.Authorization.Constants;
using Forum.API.Boards;
using Forum.API.Data;
using Forum.API.Exceptions;
using Forum.API.Extensions;
using Forum.API.ForumUsers;
using Forum.API.Interfaces;
using Forum.API.Topics.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Forum.API.Topics
{
    public class TopicsRepository(ForumDbContext dbContext, IMapper mapper, IOperationAuthorizationService authorizationService) : ITopicsRepository
    {
        public async Task<TopicDto> GetTopicByIdAsync(int id)
        {
            var topic = await dbContext.Topics
                .Include(t => t.Posts)
                .AsNoTracking()
                .SingleOrDefaultAsync(t => t.Id == id);

            if (topic is null)
            {
                throw new NotFoundException($"Read failed - couldn't find Topic with id: {id}");
            }

            TopicDto topicDto = mapper.Map<TopicDto>(topic);
            return topicDto;
        }

        public async Task<int> CreateTopicAsync(CreateTopicDto createTopicDto)
        {
            var board = await dbContext.Boards.FirstOrDefaultAsync(b => b.Id == createTopicDto.BoardId);
            if(board is null)
            {
                throw new NotFoundException($"Create failed - couldn't find Board with id: {createTopicDto.BoardId} to create topic");
            }
            Topic newTopic = mapper.Map<Topic>(createTopicDto);
            bool isAuthorized = await authorizationService.IsResourceOperationAuthorizedAsync(newTopic, ResourceOperations.Create);
            if (!isAuthorized)
            {
                throw new ForbiddenException("User doesn't have permission to create Topic");
            }

            dbContext.Topics.Add(newTopic);
            await dbContext.SaveChangesAsync();
            return newTopic.Id;
        }

        public async Task UpdateTopicAsync(int topicId, UpdateTopicDto updateTopicDto)
        {
            var topic = await dbContext.Topics.FindAsync(topicId);
            if (topic is null)
            {
                throw new NotFoundException($"Update failed - couldn't find Topic with id: {topicId}");
            }
            bool isAuthorized = await authorizationService.IsResourceOperationAuthorizedAsync(topic, ResourceOperations.Delete);
            if (!isAuthorized)
            {
                throw new ForbiddenException("User doesn't have permission to update Topic");
            }            
            var newTopic = mapper.Map(updateTopicDto, topic);
            await dbContext.SaveChangesAsync();

        }

        public async Task DeleteTopicAsync(int topicId)
        {
            var topic = await dbContext.Topics.FindAsync(topicId);
            if(topic is null)
            {
                throw new NotFoundException($"Delete failed - couldn't find Topic with id: {topicId}");
            }
            bool isAuthorized = await authorizationService.IsResourceOperationAuthorizedAsync(topic, ResourceOperations.Delete);
            if (!isAuthorized)
            {
                throw new ForbiddenException("User doesn't have permission to delete Topic");
            }
            dbContext.Topics.Remove(topic);
            await dbContext.SaveChangesAsync();
        }

    }
}
