using Forum.API.Authorization.Constants;
using Forum.API.Boards.DTOs;
using Forum.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.API.Controllers
{
    [Route("api/boards")]
    [ApiController]
    public class BoardsController(IBoardsRepository boardsRepository) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BoardDto>>> GetAllBoards()
        {
            var boardDtos = await boardsRepository.GetAllBoardsAsync();
            return Ok(boardDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BoardDto>> GetBoardById([FromRoute] int id)
        {
            var board = await boardsRepository.GetBoardByIdAsync(id);
            return Ok(board);
        }

        [HttpPost]
        [Authorize(Policy = AuthorizationPolicies.RequireModerator)]
        public async Task<ActionResult> CreateBoard([FromBody] CreateBoardDto boardDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            } 
            int id = await boardsRepository.CreateBoardAsync(boardDto);
            return CreatedAtAction(nameof(GetBoardById), new { id }, null);
        }

        [HttpPatch("{id}")]
        [Authorize(Policy = AuthorizationPolicies.RequireModerator)]
        public async Task<ActionResult> UpdateBoard([FromRoute] int id, [FromBody] UpdateBoardDto updateBoardDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(updateBoardDto.Name is null && updateBoardDto.Description is null)
            {
                ModelState.AddModelError("emptyRequest", "One of the fields must be filled");
                return BadRequest(ModelState);
            }
            await boardsRepository.UpdateBoardAsync(id, updateBoardDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = AuthorizationPolicies.RequireModerator)]
        public async Task<ActionResult> DeleteBoard([FromRoute] int id)
        {
            await boardsRepository.DeleteBoardAsync(id);
            return NoContent();
        }

    }
}
