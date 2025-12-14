using Serilog.Context;

namespace Fundo.WebApi.Middlewares;

public class CorrellationIdMiddleware
{
    private const string CorrelationIdHeaderName = "X-Correlation-Id";
    private const string CorrelationIdLogProperty = "CorrelationId";
    private readonly RequestDelegate _next;

    public CorrellationIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(CorrelationIdHeaderName, out var correlationId))
        {
            correlationId = Guid.NewGuid().ToString();
        }
        context.Response.Headers.TryAdd(CorrelationIdHeaderName, correlationId);
        using var _ = LogContext.PushProperty(CorrelationIdLogProperty, correlationId);
        await _next(context);
    }
}