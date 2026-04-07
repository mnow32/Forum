using Forum.API.Seeding;
using Scalar.AspNetCore;
using Serilog;
using Forum.API.Exceptions;
using Forum.API.Configuration;
using Forum.API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

try
{
    builder.ConfigureServices(builder.Configuration);
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Error configuring services: {ex.Message}");
    throw;
}
catch (Exception ex)
{
    Console.WriteLine($"Error configuring services: {ex.Message}");
    throw;
}


var app = builder.Build();

app.UseExceptionHandling();

// migrate database and create initial data
using var scope = app.Services.CreateScope();
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ForumDbContext>();
    var pendingMigrations = dbContext.Database.GetPendingMigrations();
    if (pendingMigrations.Any())
    {
        dbContext.Database.Migrate();
    }

    var seeder = scope.ServiceProvider.GetRequiredService<IForumSeeder>();
    try
    {
        await seeder.SeedAsync();
    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine($"Critical error during essential data seeding: {ex.Message}");
        throw;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Critical error during essential data seeding: {ex.Message}");
        throw;
    }
}

app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options
        .WithTitle("Forum API")
        .WithTheme(ScalarTheme.Kepler)
        .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);

        options
            .AddPreferredSecuritySchemes("Bearer")
            .AddHttpAuthentication(
                "Bearer",
                auth =>
                {
                    auth.Token = "";
                }
            )
            .EnablePersistentAuthentication();
    });
    
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
