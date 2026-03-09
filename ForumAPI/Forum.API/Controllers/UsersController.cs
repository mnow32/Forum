using AutoMapper;
using Forum.API.DTOs;
using Forum.API.Entities;
using Forum.API.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Forum.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController(UserManager<ForumUser> userManager, ITokenService tokenService, IMapper mapper) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult<ForumUserDto>> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }

            var user = await userManager.FindByEmailAsync(registerDto.Email);
            if (user is not null)
            {
                return BadRequest("Email already exists");
            }

            var newForumUser = mapper.Map<ForumUser>(registerDto);

            var result = await userManager.CreateAsync(newForumUser, registerDto.Password);

            if (!result.Succeeded)
            {
                //TODO: Add error logging
                //result.Errors
                return ValidationProblem();
            }


            var userDto = mapper.Map<ForumUserDto>(newForumUser);
            userDto.Token = tokenService.GenerateToken(newForumUser);

            return userDto;
        }

        [HttpPost("login")]
        public async Task<ActionResult<ForumUserDto>> Login([FromBody] LoginDto loginDto)
        {
            var user = await userManager.FindByEmailAsync(loginDto.Email);
            if(user is null)
            {
                return Unauthorized();
            }

            var result = await userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!result)
            {
                return Unauthorized();
            }

            var userDto = mapper.Map<ForumUserDto>(user);
            userDto.Token = tokenService.GenerateToken(user);

            return userDto;
        }
    }
}
