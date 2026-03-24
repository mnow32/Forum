using AutoMapper;
using Forum.API.Authorization.Constants;
using Forum.API.ForumUsers;
using Forum.API.ForumUsers.DTOs;
using Forum.API.Tokens;
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
            var newForumUser = mapper.Map<ForumUser>(registerDto);

            var result = await userManager.CreateAsync(newForumUser, registerDto.Password);

            if (!result.Succeeded)
            {
                //TODO: Add error logging
                //result.Errors
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("identity", error.Description);
                }

                return ValidationProblem();
            }

            await userManager.AddToRoleAsync(newForumUser, ForumRoles.Member);
            await SetRefreshTokenCookie(newForumUser);
            var userDto = mapper.Map<ForumUserDto>(newForumUser);
            userDto.Token = await tokenService.GenerateToken(newForumUser);

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
            userDto.Token = await tokenService.GenerateToken(user);

            return userDto;
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<ForumUserDto>> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if(refreshToken is null)
            {
                return Unauthorized(); 
            }

            string refreshTokenHash = tokenService.HashRefreshToken(refreshToken);
            var user = await userManager.Users.FirstOrDefaultAsync(u => u.RefreshTokenHash == refreshTokenHash && u.RefreshTokenExpiry > DateTime.UtcNow);
            

            if(user is null)
            {
                return Unauthorized();
            }

            await SetRefreshTokenCookie(user);
            var userDto = mapper.Map<ForumUserDto>(user);
            userDto.Token = await tokenService.GenerateToken(user);

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
