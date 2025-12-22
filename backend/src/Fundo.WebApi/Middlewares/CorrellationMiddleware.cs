namespace Fundo.WebApi.Middlewares;

public class CorrellationMiddleware
{
    private const string CorrelationIdHeaderName = "X-Correlation-Id";
    private const string CorrelationIdLogProperty = "CorrelationId";
    private const string ApplicationHeaderName = "X-Application-Name";
    private const string ApplicationLogProperty = "AppOriginName";
    private readonly ILogger<CorrellationMiddleware> _logger;
    private readonly RequestDelegate _next;

    public CorrellationMiddleware(RequestDelegate next, ILogger<CorrellationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = GetOrCreateCorrelationId(context);
        var applicationName = GetApplicationName(context);

        using var _ = _logger.BeginScope(new Dictionary<string, string>
               {
                   { CorrelationIdLogProperty, correlationId },
                   { ApplicationLogProperty, applicationName }
               });

        await _next(context);
    }

    private static string GetApplicationName(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue(ApplicationHeaderName, out var applicationName))
        {
            return applicationName.ToString();
        }
        return string.Empty;
    }

    private static string GetOrCreateCorrelationId(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(CorrelationIdHeaderName, out var correlationId))
        {
            correlationId = Guid.NewGuid().ToString();
        }
        context.Response.Headers.TryAdd(CorrelationIdHeaderName, correlationId);
        return correlationId.ToString();
    }
}