namespace API.Middlewares;

/// <summary>
///     Middleware for logging request
/// </summary>
/// <param name="next"> </param>
/// <param name="logger"> </param>
internal sealed class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        logger.LogInformation("Request Method: {Method}, Path: {Path}", context.Request.Method, context.Request.Path);

        await next(context);

        logger.LogInformation("Response StatusCode: {StatusCode}", context.Response.StatusCode);
    }
}