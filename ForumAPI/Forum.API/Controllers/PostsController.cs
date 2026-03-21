using Forum.API.Extensions;
using Forum.API.Interfaces;
using Forum.API.Posts.DTOs;
using Forum.API.Topics;
using Forum.API.Topics.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
