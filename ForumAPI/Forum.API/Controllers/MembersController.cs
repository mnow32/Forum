using Forum.API.Extensions;
using Forum.API.ForumMembers;
using Forum.API.ForumMembers.DTOs;
using Forum.API.Pagination;
using Forum.API.Pagination.Params;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.API.Controllers
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
    }
}
