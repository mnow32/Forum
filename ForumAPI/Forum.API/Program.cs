using Forum.API.Seeding;
using Scalar.AspNetCore;
using Serilog;
using Forum.API.Exceptions;
using Forum.API.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureServices(builder.Configuration);

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
