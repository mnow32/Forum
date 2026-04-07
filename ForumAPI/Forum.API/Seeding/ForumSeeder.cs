using Forum.API.Authentication.ForumUsers;
using Forum.API.Authorization.Constants;
using Forum.API.Data;
using Microsoft.AspNetCore.Identity;

namespace Forum.API.Seeding
{
    internal class ForumSeeder(ForumDbContext dbcontext, UserManager<ForumUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, ILogger<ForumSeeder> logger) : IForumSeeder
    {
        public async Task SeedAsync()
        {
            if(await dbcontext.Database.CanConnectAsync())
            {
                // seed user roles
                if (!roleManager.Roles.Any())
                {
                    var roles = GetRoles();

                    foreach (var role in roles)
                    {
                        var result = await roleManager.CreateAsync(role);
                        if(!result.Succeeded)
                        {
                            var errors = result.Errors.Select(error => error.Description);
                            var message = string.Join(", ", errors);
                            logger.LogWarning(message);
                        }
                    }
                }

                // create admin user
                if(!userManager.Users.Any())
                {
                    var adminName = configuration["Forum:AdminAccount:DisplayName"]
                        ?? throw new InvalidOperationException("Couldn't retrieve Admin name from configuration file");

                    var adminEmail = configuration["Forum:AdminAccount:Email"] 
                        ?? throw new InvalidOperationException("Couldn't retrieve Admin email from configuration file");

                    var adminPassword = configuration["Forum:AdminAccount:Password"] 
                        ?? throw new InvalidOperationException("Couldn't retrieve Admin password from configuration file");

                    var adminUser = new ForumUser() { DisplayName = adminName, UserName = adminEmail, Email = adminEmail };

                    var createAdminResult = await userManager.CreateAsync(adminUser, adminPassword);

                    if(!createAdminResult.Succeeded)
                    {
                        var errors = createAdminResult.Errors.Select(error => error.Description);
                        var message = string.Join(", ", errors);
                        logger.LogWarning(message);
                    }

                    var addRoleResult = await userManager.AddToRoleAsync(adminUser, ForumRoles.Administrator);

                    if (!addRoleResult.Succeeded)
                    {
                        var errors = createAdminResult.Errors.Select(error => error.Description);
                        var message = string.Join(", ", errors);
                        logger.LogWarning(message);
                    }
                }

            }
            else
            {
                logger.LogError("Seeder was unable to create initial data - couldn't connect to database");
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
