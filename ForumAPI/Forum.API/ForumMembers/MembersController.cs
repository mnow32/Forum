using Forum.API.Authentication;
using Forum.API.ForumMembers.DTOs;
using Forum.API.Pagination;
using Forum.API.Pagination.Params;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.API.ForumMembers
{    
    [ApiController]
    public class MembersController(IForumMembersRepository forumMembersRepository) : ControllerBase
    {
        [HttpGet("api/members")]
        [Authorize]
        public async Task<ActionResult<PaginationResult<ForumMemberDto>>> GetAllMembers([FromQuery] MemberParams memberParams)
        {
            memberParams.CurrentMemberId = User.GetMemberId();
            var pagedResult = await forumMembersRepository.GetMembersAsync(memberParams);
            return Ok(pagedResult);
        }

        [HttpPatch("api/members")]
        [Authorize]
        public async Task<ActionResult> UpdateMember([FromForm] UpdateForumMemberDto updateForumMemberDto)
        {
            updateForumMemberDto.Id = User.GetMemberId();
            await forumMembersRepository.UpdateMemberAsync(updateForumMemberDto);
            return NoContent();
        } 
    }
}
