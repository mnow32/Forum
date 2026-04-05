using Azure.Core;
using Forum.API.Exceptions.Models;
using Microsoft.AspNetCore.Mvc;

namespace Forum.API.Exceptions
{
    public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (TokenException ex)
            {
                logger.LogError(ex, ex.Message);
                context.Response.StatusCode = 401;

                await context.Response.WriteAsync("You need to authenticate");
            }
            catch (ForbiddenException ex)
            {
                logger.LogWarning(ex.Message);
                context.Response.StatusCode = 403;

                await context.Response.WriteAsync("Yoou cannot operate on this resource");
            }
            catch (NotFoundException ex)
            {
                logger.LogError(ex.Message);
                context.Response.StatusCode = 404;

                await context.Response.WriteAsync("Couldn't find resource");
            }
            catch (CloudinaryException ex)
            {
                logger.LogError(ex.Message);
                context.Response.StatusCode = 500;

                await context.Response.WriteAsync("Something went wrong");
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
