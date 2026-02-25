using Forum.API.Entities;
using Forum.API.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Forum.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoardsController(IBoardsRepository boardsRepository) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllBoards()
        {
            var boards = await boardsRepository.GetAllBoardsAsync();
            return Ok(boards);
        }

        [HttpGet]
        [Route("/{id}")]
        public async Task<IActionResult> GetBoardById([FromRoute] int id)
        {
            var board = await boardsRepository.GetBoardByIdAsync(id);
            return Ok(board);
        }
    }
}
