using Forum.API.Extensions;
using Forum.API.Replies;
using Forum.API.Replies.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.API.Controllers
{
    [ApiController]
    public class RepliesController(IRepliesRepository repliesRepository) : ControllerBase
    {
        [HttpGet("api/posts/{id}/replies")]
        public async Task<ActionResult<IEnumerable<ReplyDto>>> GetPostReplies([FromRoute] int postId)
        {
            IEnumerable<ReplyDto> replies = await repliesRepository.GetRepliesByPostIdAsync(postId);
            return Ok(replies);
        }

        [HttpPost("api/posts/{id}/replies")]
        [Authorize]
        public async Task<ActionResult> CreateReply([FromRoute] int postId, [FromBody] CreateReplyDto createReplyDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            createReplyDto.PostId = postId;
            createReplyDto.MemberId = User.GetMemberId();
            createReplyDto.MemberName = User.GetMemberName();
            int id = await repliesRepository.CreateReplyAsync(createReplyDto);
            return Created();
        }

        [HttpPatch("api/replies/{id}")]
        [Authorize]
        public async Task<ActionResult> UpdateReply([FromRoute] int replyId, [FromBody] UpdateReplyDto updateReplyDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await repliesRepository.UpdateReplyAsync(replyId, updateReplyDto);
            return NoContent();
        }

        [HttpDelete("api/replies/{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteReply([FromRoute] int replyId)
        {
            await repliesRepository.DeleteReplyAsync(replyId);
            return NoContent();
        }
    }
}
