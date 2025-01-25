using Microsoft.AspNetCore.Diagnostics;

namespace WorkHunter.Api.Middleware;

public sealed class ExceptionHandler(ILogger<ExceptionHandler> logger) : IExceptionHandler
{
    private readonly ILogger<ExceptionHandler> logger = logger;

    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken token)
    {
        var message = exception.GetBaseException().Message;
        logger.LogError(exception, message);

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsync(message, token);

        return true;
    }
}
