using AutoMapper;
using Forum.API.Boards;
using Forum.API.Exceptions;
using Forum.API.Extensions;
using Forum.API.ForumUsers;
using Forum.API.Interfaces;
using Forum.API.Topics;
using Forum.API.Topics.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Forum.API.Data.Repositories
{
    public class TopicsRepository(ForumDbContext dbContext, IMapper mapper) : ITopicsRepository
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
            var member = await dbContext.Members.FirstOrDefaultAsync(m => m.Id == createTopicDto.MemberId);
            if(board is null)
            {
                throw new NotFoundException($"Create failed - couldn't find Board with id: {createTopicDto.BoardId} to create topic");
            }
            if(member is null)
            {
                throw new NotFoundException($"Create failed - couldn't find Member to create topic");
            }

            Topic newTopic = mapper.Map<Topic>(createTopicDto);
            dbContext.Topics.Add(newTopic);
            await dbContext.SaveChangesAsync();
            return newTopic.Id;
        }

        public async Task UpdateTopicAsync(int topicId, UpdateTopicDto updateTopicDto, ClaimsPrincipal user)
        {

        }

        public async Task DeleteTopicAsync(int id, ClaimsPrincipal user)
        {

        }

    }
}
