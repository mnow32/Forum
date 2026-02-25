using Forum.API.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Forum.API.Controllers
{
    [ApiController]
    public class PostsController(IPostsRepository postsRepository) : ControllerBase
    {
        [HttpGet]
        [Route("api/topics/{id}/posts")]
        public async Task<IActionResult> GetAllPostsByTopicId([FromRoute] int id)
        {
            var posts = await postsRepository.GetAllPostsByTopicIdAsync(id);
            return Ok(posts);
        }
    }
}
