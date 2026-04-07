using Forum.API.Authentication;
using Forum.API.Authorization.Constants;
using Forum.API.Pagination;
using Forum.API.Pagination.Params;
using Forum.API.Topics.DTOs;
using Forum.API.Topics.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.API.Topics
{
    [ApiController]
    public class TopicsController(ITopicsRepository topicsRepository) : ControllerBase
    {
        
        [HttpGet("api/topics/{topicId}")]
        public async Task<ActionResult<TopicDto>> GetTopicById([FromRoute] int topicId)
        {
            TopicDto topicDto = await topicsRepository.GetTopicByIdAsync(topicId);
            return Ok(topicDto);
        }

        [HttpGet("api/boards/{boardId}/topics")]
        public async Task<ActionResult<PaginationResult<TopicDto>>> GetTopicsForBoard([FromRoute] int boardId, [FromQuery] TopicParams topicParams)
        {
            var pagedResult = await topicsRepository.GetBoardTopicsByIdAsync(boardId, topicParams);
            return Ok(pagedResult);
        }

        [HttpPost("api/boards/{boardId}/topics")]
        [Authorize(Policy = AuthorizationPolicies.RequireMember)]
        public async Task<ActionResult> CreateTopic([FromRoute] int boardId, [FromForm] CreateTopicDto createTopicDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            createTopicDto.BoardId = boardId;
            createTopicDto.MemberId = User.GetMemberId();
            createTopicDto.MemberName = User.GetMemberName();
            int id = await topicsRepository.CreateTopicAsync(createTopicDto);
            return CreatedAtAction(nameof(GetTopicById), new { id }, null);
        }

        [HttpPatch("api/topics/{topicId}")]
        [Authorize]
        public async Task<ActionResult> UpdateTopic([FromRoute] int topicId, [FromBody] UpdateTopicDto updateTopicDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (updateTopicDto.Title is null && updateTopicDto.Description is null)
            {
                ModelState.AddModelError("emptyRequest", "One of the fields must be filled");
                return BadRequest(ModelState);
            }
            await topicsRepository.UpdateTopicAsync(topicId, updateTopicDto);
            return NoContent();
        }

        [HttpDelete("api/topics/{topicId}")]
        [Authorize]
        public async Task<ActionResult> DeleteTopic([FromRoute] int topicId)
        {
            await topicsRepository.DeleteTopicAsync(topicId);
            return NoContent();
        }

    }
}
