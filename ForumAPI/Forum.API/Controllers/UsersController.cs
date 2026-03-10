using AutoMapper;
using Forum.API.DTOs;
using Forum.API.Entities;
using Forum.API.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

            await SetRefreshTokenCookie(user);
            var userDto = mapper.Map<ForumUserDto>(user);
            userDto.Token = tokenService.GenerateToken(user);

            return userDto;
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<ForumUserDto>> RefreshToken()
        {
            var token = Request.Cookies["refreshToken"];
            if(token is null)
            {
                return Unauthorized(); 
            }

            string tokenHash = tokenService.HashRefreshToken(token);
            var user = await userManager.Users.FirstOrDefaultAsync(u => u.RefreshTokenHash == tokenHash && u.RefreshTokenExpiry < DateTime.UtcNow);

            if(user is null)
            {
                return Unauthorized();
            }

            await SetRefreshTokenCookie(user);
            var userDto = mapper.Map<ForumUserDto>(user);
            userDto.Token = tokenService.GenerateToken(user);

            return userDto;
        }

        private async Task SetRefreshTokenCookie(ForumUser user)
        {
            string refreshToken = tokenService.GenerateRefreshToken();
            string hashedToken = tokenService.HashRefreshToken(refreshToken);

            user.RefreshTokenHash = hashedToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            await userManager.UpdateAsync(user);

            var cookieOptions = new CookieOptions()
            {
                Expires = DateTime.UtcNow.AddDays(7),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            };

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
    }
}
