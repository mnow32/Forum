using AutoMapper;
using Forum.API.Authorization;
using Forum.API.Authorization.Constants;
using Forum.API.Data;
using Forum.API.Exceptions;
using Forum.API.Pagination;
using Forum.API.Pagination.Params;
using Forum.API.Topics.DTOs;
using Microsoft.EntityFrameworkCore;

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

        public async Task<PaginationResult<TopicDto>> GetBoardTopicsByIdAsync(int boardId, TopicParams topicParams)
        {
            var board = await dbContext.Boards.FirstOrDefaultAsync(b => b.Id == boardId);
            if(board is null)
            {
                throw new NotFoundException($"Read failed - couldn't find Board with id: {boardId} to retrieve Topics");
            }
            dbContext.Entry(board).State = EntityState.Detached;

            var query = dbContext.Topics.AsQueryable().AsNoTracking();
            if(topicParams.Title.Length > 0)
            {
                query = query.Where(t => t.Title.Contains(topicParams.Title));
            }
            query = query.OrderBy(t => t.CreatedAt);

            (PaginationMetadata metadata, List<Topic> topics) = await PaginationHelper.CreatePagingAsync(query, topicParams.PageNumber, topicParams.PageSize);

            return new PaginationResult<TopicDto>()
            {
                Metadata = metadata,
                Items = mapper.Map<List<TopicDto>>(topics)
            };

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
                throw new ForbiddenException("Create failed - User doesn't have permission to create Topic");
            }

            dbContext.Topics.Add(newTopic);
            await dbContext.SaveChangesAsync();
            return newTopic.Id;
        }

        public async Task UpdateTopicAsync(int topicId, UpdateTopicDto updateTopicDto)
        {
            var topic = await dbContext.Topics.FirstOrDefaultAsync(t => t.Id == topicId);
            if (topic is null)
            {
                throw new NotFoundException($"Update failed - couldn't find Topic with id: {topicId}");
            }
            bool isAuthorized = await authorizationService.IsResourceOperationAuthorizedAsync(topic, ResourceOperations.Update);
            if (!isAuthorized)
            {
                throw new ForbiddenException("Update failed - User doesn't have permission to update Topic");
            }            
            mapper.Map(updateTopicDto, topic);
            await dbContext.SaveChangesAsync();

        }

        public async Task DeleteTopicAsync(int topicId)
        {
            var topic = await dbContext.Topics.FirstOrDefaultAsync(t => t.Id == topicId);
            if(topic is null)
            {
                throw new NotFoundException($"Delete failed - couldn't find Topic with id: {topicId}");
            }
            bool isAuthorized = await authorizationService.IsResourceOperationAuthorizedAsync(topic, ResourceOperations.Delete);
            if (!isAuthorized)
            {
                throw new ForbiddenException("Delete failed - User doesn't have permission to delete Topic");
            }
            dbContext.Topics.Remove(topic);
            await dbContext.SaveChangesAsync();
        }

    }
}
