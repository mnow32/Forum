using Forum.API.Data;
using Forum.API.Interfaces;
using Forum.API.Seeding;
using Forum.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Reflection;
using System.Text;
using Microsoft.IdentityModel.Protocols.Configuration;
using Serilog;
using Forum.API.Extensions;
using Forum.API.Authorization;
using Forum.API.Authorization.Constants;
using Forum.API.ForumUsers;
using Forum.API.Topics;
using Forum.API.Posts;
using Forum.API.Boards;
using Forum.API.Replies;
using Forum.API.ForumMembers;
using Forum.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration["ConnectionStrings:ForumDbConnection"] 
    ?? throw new InvalidConfigurationException("Could not retrieve database connection string from configuration file");
var automapperLicenseKey = builder.Configuration["Forum:AutomapperLicenseKey"] 
    ?? throw new InvalidConfigurationException("Could not retrieve Automapper license key from configuration file");
var tokenKey = builder.Configuration["Forum:TokenKey"] 
    ?? throw new InvalidConfigurationException("Could not retrieve token key from configuration file");

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<ForumDbContext>(options => options.UseSqlServer(connectionString).EnableSensitiveDataLogging());
builder.Services.AddScoped<IBoardsRepository, BoardsRepository>();
builder.Services.AddScoped<ITopicsRepository, TopicsRepository>();
builder.Services.AddScoped<IPostsRepository, PostsRepository>();
builder.Services.AddScoped<IRepliesRepository, RepliesRepository>();
builder.Services.AddScoped<IForumMembersRepository, ForumMembersRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
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
    .AddPolicy(AuthorizationPolicies.RequireModerator, policy => policy.RequireRole(ForumRoles.Administrator, ForumRoles.Moderator));

builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});


var app = builder.Build();

app.UseExceptionHandling();

using var scope = app.Services.CreateScope();
{
    var seeder = scope.ServiceProvider.GetRequiredService<IForumSeeder>();
    await seeder.SeedAsync();
}


app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
