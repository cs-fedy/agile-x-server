using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace AgileX.Api.Middleware;

public class ErrorHandlingMiddleware : IMiddleware
{
    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        ProblemDetails details =
            new()
            {
                Status = (int)context.Response.StatusCode,
                Title = exception.Message,
                Detail = exception.StackTrace
            };

        var result = JsonSerializer.Serialize(details);
        context.Response.ContentType = "application/json";
        return context.Response.WriteAsync(result);
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            Results.Ok(ex);
            await HandleExceptionAsync(context, ex);
        }
    }
}
