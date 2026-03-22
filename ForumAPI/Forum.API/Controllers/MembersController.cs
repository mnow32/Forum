using Forum.API.ForumMembers;
using Forum.API.ForumMembers.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.API.Controllers
{    
    [ApiController]
    public class MembersController(IForumMembersRepository forumMembersRepository) : ControllerBase
    {
        [HttpGet("api/members")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ForumMemberDto>>> GetAllMembers()
        {
            var memberDtos = await forumMembersRepository.GetAllMembersAsync();
            return Ok(memberDtos);
        }
    }
}
