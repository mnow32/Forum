using Forum.API.Authentication.ForumUsers;
using Forum.API.Authentication.Tokens;
using Forum.API.Authorization;
using Forum.API.Authorization.Constants;
using Forum.API.Boards.Repository;
using Forum.API.Data;
using Forum.API.ForumMembers.Repository;
using Forum.API.Photos;
using Forum.API.Posts.Repository;
using Forum.API.Replies.Repository;
using Forum.API.Seeding;
using Forum.API.Topics.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Reflection;
using System.Text;

namespace Forum.API.Configuration
{
    public static class WebApplicationBuilderExtensions
    {
        public static void ConfigureServices(this WebApplicationBuilder builder, IConfiguration configuration)
        {
            var connectionString = builder.Configuration["ConnectionStrings:ForumDbConnection"]
                ?? throw new InvalidOperationException("Could not retrieve database connection string from configuration file");

            var automapperLicenseKey = builder.Configuration["Forum:AutomapperLicenseKey"] 
                ?? throw new InvalidOperationException("Could not retrieve Automapper license key from configuration file");

            var tokenKey = builder.Configuration["Forum:TokenKey"]
                ?? throw new InvalidOperationException("Could not retrieve token key from configuration file");

            var cloudinarySettings = builder.Configuration.GetSection("Forum:CloudinarySettings");
            if (!cloudinarySettings.Exists())
            {
                throw new InvalidOperationException("Could not retrieve Cloudinary settings from configuration file");
            }

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi("v1", options => { options.AddDocumentTransformer<BearerSecuritySchemeTransformer>(); });
            builder.Services.Configure<CloudinarySettings>(cloudinarySettings);
            builder.Services.AddDbContext<ForumDbContext>(options => options.UseSqlServer(connectionString).EnableSensitiveDataLogging());
            builder.Services.AddScoped<IBoardsRepository, BoardsRepository>();
            builder.Services.AddScoped<ITopicsRepository, TopicsRepository>();
            builder.Services.AddScoped<IPostsRepository, PostsRepository>();
            builder.Services.AddScoped<IRepliesRepository, RepliesRepository>();
            builder.Services.AddScoped<IForumMembersRepository, ForumMembersRepository>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IPhotoService, PhotoService>();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<IOperationAuthorizationService, OperationAuthorizationService>();
            builder.Services.AddScoped<IForumSeeder, ForumSeeder>();
            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.LicenseKey = automapperLicenseKey;
            }, Assembly.GetExecutingAssembly());

            builder.Services.AddIdentityCore<ForumUser>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;
            })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ForumDbContext>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(jwtOptions =>
                {
                    jwtOptions.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            builder.Services.AddAuthorizationBuilder()
                .AddPolicy(AuthorizationPolicies.RequireAdministrator, policy => policy.RequireRole(ForumRoles.Administrator))
                .AddPolicy(AuthorizationPolicies.RequireModerator, policy => policy.RequireRole(ForumRoles.Administrator, ForumRoles.Moderator))
                .AddPolicy(AuthorizationPolicies.RequireMember, policy => policy.RequireRole(ForumRoles.Member));

            builder.Host.UseSerilog((context, configuration) =>
            {
                configuration.ReadFrom.Configuration(context.Configuration);
            });
        }
    }
}
