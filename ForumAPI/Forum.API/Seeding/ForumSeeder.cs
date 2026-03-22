using Forum.API.Authorization.Constants;
using Forum.API.Data;
using Microsoft.AspNetCore.Identity;

namespace Forum.API.Seeding
{
    internal class ForumSeeder(RoleManager<IdentityRole> roleManager) : IForumSeeder
    {
        public async Task SeedAsync()
        {

            if (!roleManager.Roles.Any())
            {
                var roles = GetRoles();
                foreach (var role in roles)
                {
                    await roleManager.CreateAsync(role);
                }
            }

        }

        private static IEnumerable<IdentityRole> GetRoles()
        {
            return new List<IdentityRole>
            {
                new IdentityRole(ForumRoles.Member)
                {
                    NormalizedName = ForumRoles.Member.ToUpper(),
                },
                new IdentityRole(ForumRoles.Moderator)
                {
                    NormalizedName = ForumRoles.Moderator.ToUpper(),
                },
                new IdentityRole(ForumRoles.Administrator)
                {
                    NormalizedName = ForumRoles.Administrator.ToUpper(),
                }
            };
        }
    }
}
