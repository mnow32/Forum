using Forum.API.Boards.DTOs;
using Forum.API.Extensions;
using Forum.API.Interfaces;
using Forum.API.Topics.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

namespace Forum.API.Controllers
{
    [ApiController]
    public class TopicsController(ITopicsRepository topicsRepository) : ControllerBase
    {
        
        [HttpGet("api/topics/{id}")]
        public async Task<ActionResult<TopicDto>> GetTopicById([FromRoute] int id)
        {
            TopicDto topicDto = await topicsRepository.GetTopicByIdAsync(id);
            return Ok(topicDto);
        }

        [HttpPost("api/boards/{boardId}/topics")]
        [Authorize]
        public async Task<ActionResult> CreateTopic([FromRoute] int boardId, [FromBody] CreateTopicDto createTopicDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            createTopicDto.BoardId = boardId;
            createTopicDto.MemberId = User.GetMemberId();
            int id = await topicsRepository.CreateTopicAsync(createTopicDto);
            return CreatedAtAction(nameof(GetTopicById), new { id }, null);
        }

        [HttpPatch("api/topics/{id}")]
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

        [HttpDelete("api/topics/{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteTopic([FromRoute] int topicId)
        {
            await topicsRepository.DeleteTopicAsync(topicId);
            return NoContent();
        }

    }
}
