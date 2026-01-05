using Fundo.WebApi.Middlewares;

namespace Fundo.WebApi.Extensions;

public static class CustomMiddlewareExtensions
{
    public static void UseCustomMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<CorrellationMiddleware>();
        app.UseMiddleware<ErrorHandlingMiddleware>();
    }
}