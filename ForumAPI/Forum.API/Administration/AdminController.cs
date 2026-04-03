using Forum.API.Authentication.ForumUsers;
using Forum.API.Authorization.Constants;
using Forum.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Forum.API.Administration
{    
    [ApiController]
    public class AdminController(UserManager<ForumUser> userManager, ForumDbContext dbContext) : ControllerBase
    {
        [HttpGet("api/admin/users")]
        //[Authorize(Policy = AuthorizationPolicies.RequireAdministrator)]
        public async Task<ActionResult<IEnumerable<object>>> GetUsersWithRoles()
        {
            var usersWithRoles = await (from user in dbContext.Users
                               select new
                               {
                                   Id = user.Id,
                                   DisplayName = user.DisplayName,
                                   Email = user.Email,
                                   Roles = (from userRole in dbContext.UserRoles
                                            join role in dbContext.Roles on userRole.RoleId equals role.Id
                                            where userRole.UserId == user.Id
                                            select role.Name).AsEnumerable()
                               }).ToListAsync();

            return Ok(usersWithRoles);
        }

        [HttpPost("api/admin/users/{userId}/assign-roles")]
        //[Authorize(Policy = AuthorizationPolicies.RequireAdministrator)]
        public async Task<ActionResult<IEnumerable<string>>> AddToRoles([FromRoute] string userId, [FromBody] List<string> newRoles)
        {
            List<string> acceptedRoles = new()
            {
                ForumRoles.Member,
                ForumRoles.Moderator,
                ForumRoles.Administrator
            };

            if(newRoles is null || newRoles.Count == 0)
            {
                return BadRequest("You must select at least one role.");
            }

            foreach (var role in newRoles)
            {
                if (!acceptedRoles.Contains(role))
                {
                    return BadRequest($"{role} is not an accepted value");
                }
            }

            var user = await userManager.FindByIdAsync(userId);

            if(user is null)
            {
                return BadRequest("Couldn't retrieve user.");
            }

            var userRoles = await userManager.GetRolesAsync(user);

            var result = await userManager.AddToRolesAsync(user, newRoles.Except(userRoles));

            if (!result.Succeeded)
            {
                return BadRequest("Failed to add roles.");
            }

            var updatedRoles = await userManager.GetRolesAsync(user);

            return Ok(updatedRoles);
        }

        [HttpPost("api/admin/users/{userId}/unassign-roles")]
        //[Authorize(Policy = AuthorizationPolicies.RequireAdministrator)]
        public async Task<ActionResult<IEnumerable<string>>> RemoveFromRoles([FromRoute] string userId, [FromBody] List<string> rolesToRemove)
        {
            List<string> acceptedRoles = new()
            {
                ForumRoles.Member,
                ForumRoles.Moderator,
                ForumRoles.Administrator
            };

            if (rolesToRemove is null || rolesToRemove.Count == 0)
            {
                return BadRequest("You must select at least one role.");
            }

            var user = await userManager.FindByIdAsync(userId);

            if (user is null)
            {
                return BadRequest("Couldn't retrieve user.");
            }

            var userRoles = await userManager.GetRolesAsync(user);

            foreach (var role in rolesToRemove)
            {
                if (!acceptedRoles.Contains(role))
                {
                    return BadRequest($"{role} is not an accepted value");
                }
            }

            var result = await userManager.RemoveFromRolesAsync(user, userRoles.Intersect(rolesToRemove));

            if (!result.Succeeded)
            {
                return BadRequest("Failed to add roles.");
            }

            var updatedRoles = await userManager.GetRolesAsync(user);

            return Ok(updatedRoles);
        }
    }
}
