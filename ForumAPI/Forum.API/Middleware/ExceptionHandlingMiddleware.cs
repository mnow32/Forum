using Azure.Core;
using Forum.API.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Forum.API.Middleware
{
    public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (TokenException ex)
            {
                logger.LogError(ex, ex.Message);
                context.Response.StatusCode = 401;

                await context.Response.WriteAsync("You need to authenticate");
            }
            catch (NotFoundException ex)
            {
                logger.LogError(ex, ex.Message);
                context.Response.StatusCode = 404;

                await context.Response.WriteAsync("Couldn't find resource");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                context.Response.StatusCode = 500;

                await context.Response.WriteAsync("Something went wrong");
            }
        }
    }
}
