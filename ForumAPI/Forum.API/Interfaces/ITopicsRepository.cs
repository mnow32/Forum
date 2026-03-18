using Forum.API.Topics;
using Forum.API.Topics.DTOs;
using System.Security.Claims;

namespace Forum.API.Interfaces
{
    public interface ITopicsRepository
    {
        Task<TopicDto> GetTopicByIdAsync(int id);
        Task<int> CreateTopicAsync(CreateTopicDto createTopicDto);
        Task UpdateTopicAsync(int topicId, UpdateTopicDto updateTopicDto, ClaimsPrincipal user);
        Task DeleteTopicAsync(int id, ClaimsPrincipal user);

    }
}