using Forum.API.Authorization.Constants;
using Forum.API.ForumUsers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Forum.API.Controllers
{    
    [ApiController]
    public class AdminController(UserManager<ForumUser> userManager) : ControllerBase
    {
        [HttpGet("api/admin/users")]
        //[Authorize(Policy = AuthorizationPolicies.RequireAdministrator)]
        public async Task<ActionResult<IEnumerable<object>>> GetUsersWithRoles()
        {
            var users = await userManager.Users.ToListAsync();
            var userList = new List<object>();

            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);
                userList.Add(new
                {
                    user.Id,
                    user.DisplayName,
                    user.Email,
                    Roles = roles.ToList()
                });
            }

            return Ok(userList);
        }

        [HttpPost("api/admin/users/{userId}/assign-roles")]
        //[Authorize(Policy = AuthorizationPolicies.RequireAdministrator)]
        public async Task<ActionResult> AddToRoles([FromRoute] string userId, [FromBody] List<string> newRoles)
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
        public async Task<ActionResult<IEnumerable<object>>> RemoveFromRoles([FromRoute] string userId, [FromBody] List<string> rolesToRemove)
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
                //if (!userRoles.Contains(role))
                //{
                //    return BadRequest($"User is not in role: {role}");
                //}
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
