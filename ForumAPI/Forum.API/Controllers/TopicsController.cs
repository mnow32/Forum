using Forum.API.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Forum.API.Controllers
{
    [Route("api/topics")]
    [ApiController]
    public class TopicsController(ITopicRepository topicRepository) : ControllerBase
    {
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetTopicById([FromRoute] int id)
        {
            var topic = await topicRepository.GetTopicByIdAsync(id);
            return Ok(topic);
        }
    }
}
