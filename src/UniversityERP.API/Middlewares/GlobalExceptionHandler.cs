using System.Net;
using UniversityERP.Infrastructure.Dtos;

namespace UniversityERP.API.Middlewares;

public class GlobalExceptionHandler
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly IHostEnvironment _env;

    public GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger, IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception. TraceId: {TraceId}", context.TraceIdentifier);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var message = _env.IsDevelopment()
                ? $"{ex.GetType().Name}: {ex.Message}"
                : "Internal Server Error";

            // include inner exception message in development (often the real DB error)
            if (_env.IsDevelopment() && ex.InnerException is not null)
                message += $" | Inner: {ex.InnerException.GetType().Name}: {ex.InnerException.Message}";

            var result = new ResultDto(500, false, message);

            // optional traceId
            // (only if you want it; if not, remove)
            // result.Message += $" | TraceId: {context.TraceIdentifier}";

            await context.Response.WriteAsJsonAsync(result);
        }
    }
}