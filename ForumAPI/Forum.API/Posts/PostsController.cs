using Forum.API.Authentication;
using Forum.API.Boards;
using Forum.API.Pagination;
using Forum.API.Pagination.Params;
using Forum.API.Posts.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.API.Posts
{
    [ApiController]
    public class PostsController(IPostsRepository postsRepository) : ControllerBase
    {
        [HttpGet("api/topics/{topicId}/posts")]
        public async Task<ActionResult<PaginationResult<PostDto>>> GetPostsForTopic([FromRoute] int topicId, [FromQuery] PagingParams pagingParams)
        {
            var pagedResult = await postsRepository.GetTopicPostsByIdAsync(topicId, pagingParams);
            return Ok(pagedResult);
        }

        [HttpPost("api/topics/{topicId}/posts")]
        [Authorize]
        public async Task<ActionResult> CreatePost([FromRoute] int topicId, [FromForm] CreatePostDto createPostDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            createPostDto.TopicId = topicId;
            createPostDto.MemberId = User.GetMemberId();
            createPostDto.MemberName = User.GetMemberName();
            int id = await postsRepository.CreatePostAsync(createPostDto);
            return CreatedAtAction(nameof(GetPostsForTopic), new { topicId }, null);
        }

        [HttpPatch("api/posts/{postId}")]
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

        [HttpDelete("api/posts/{postId}")]
        [Authorize]
        public async Task<ActionResult> DeletePost([FromRoute] int postId)
        {
            await postsRepository.DeletePostAsync(postId);
            return NoContent();
        }


    }
}
