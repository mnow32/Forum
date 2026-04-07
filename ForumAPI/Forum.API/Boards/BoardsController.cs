using Forum.API.Authorization.Constants;
using Forum.API.Boards.DTOs;
using Forum.API.Boards.Repository;
using Forum.API.Pagination;
using Forum.API.Pagination.Params;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.API.Boards
{
    [Route("api/boards")]
    [ApiController]
    public class BoardsController(IBoardsRepository boardsRepository) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<PaginationResult<BoardDto>>> GetAllBoards([FromQuery] BoardParams boardParams)
        {
            var pagedResult = await boardsRepository.GetBoardsAsync(boardParams);
            return Ok(pagedResult);
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
            return CreatedAtAction(nameof(GetAllBoards), null, null);
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
