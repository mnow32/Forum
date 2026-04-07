using Forum.API.Pagination;
using Forum.API.Pagination.Params;
using Forum.API.Topics.DTOs;
using System.Security.Claims;

namespace Forum.API.Topics.Repository
{
    public interface ITopicsRepository
    {
        Task<TopicDto> GetTopicByIdAsync(int id);
        Task<PaginationResult<TopicDto>> GetBoardTopicsByIdAsync(int boardId, TopicParams topicParams);
        Task<int> CreateTopicAsync(CreateTopicDto createTopicDto);
        Task UpdateTopicAsync(int topicId, UpdateTopicDto updateTopicDto);
        Task DeleteTopicAsync(int id);

    }
}