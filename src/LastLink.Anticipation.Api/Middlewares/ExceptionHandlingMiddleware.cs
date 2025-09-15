using Microsoft.AspNetCore.Mvc;

namespace LastLink.Anticipation.Api.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Domain conflict");
            await WriteProblem(context, StatusCodes.Status409Conflict, "Conflict", ex.Message);
        }
        catch (ArgumentException ex)
        {
            _logger.LogInformation(ex, "Bad request");
            await WriteProblem(context, StatusCodes.Status400BadRequest, "Bad Request", ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogInformation(ex, "Not found");
            await WriteProblem(context, StatusCodes.Status404NotFound, "Not Found", ex.Message);
        }
    }

    private static async Task WriteProblem(HttpContext ctx, int statusCode, string title, string detail)
    {
        ctx.Response.StatusCode = statusCode;
        ctx.Response.ContentType = "application/problem+json";
        var pd = new ProblemDetails { Status = statusCode, Title = title, Detail = detail };
        await ctx.Response.WriteAsJsonAsync(pd);
    }
}
