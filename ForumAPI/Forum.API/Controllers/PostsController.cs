using Forum.API.Boards;
using Forum.API.Extensions;
using Forum.API.Pagination;
using Forum.API.Pagination.Params;
using Forum.API.Posts;
using Forum.API.Posts.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.API.Controllers
{
    [ApiController]
    public class PostsController(IPostsRepository postsRepository) : ControllerBase
    {
        [HttpGet("api/posts/{id}")]
        public async Task<ActionResult<PostDto>> GetPostById([FromRoute] int id)
        {
            PostDto postDto = await postsRepository.GetPostByIdAsync(id);
            return Ok(postDto);
        }

        [HttpGet("api/topics/{id}/posts")]
        public async Task<ActionResult<PaginationResult<PostDto>>> GetPostsForTopic([FromRoute] int topicId, [FromQuery] PagingParams pagingParams)
        {
            var pagedResult = postsRepository.GetTopicPostsByIdAsync(topicId, pagingParams);
            return Ok(pagedResult);
        }

        [HttpPost("api/topics/{id}/posts")]
        [Authorize]
        public async Task<ActionResult> CreatePost([FromRoute] int topicId, [FromBody] CreatePostDto createPostDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            createPostDto.TopicId = topicId;
            createPostDto.MemberId = User.GetMemberId();
            createPostDto.MemberName = User.GetMemberName();
            int id = await postsRepository.CreatePostAsync(createPostDto);
            return CreatedAtAction(nameof(GetPostById), new { id }, null);
        }

        [HttpPatch("api/posts/{id}")]
        [Authorize]
        public async Task<ActionResult> UpdatePost([FromRoute] int postId, [FromBody] UpdatePostDto updatePostDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await postsRepository.UpdatePostAsync(postId, updatePostDto);
            return NoContent();
        }

        [HttpDelete("api/posts/{id}")]
        [Authorize]
        public async Task<ActionResult> DeletePost([FromRoute] int postId)
        {
            await postsRepository.DeletePostAsync(postId);
            return NoContent();
        }


    }
}
