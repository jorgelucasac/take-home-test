namespace Fundo.WebApi.Extensions.Swagger;

public static class SwaggerExtensions
{
    public static void AddSwaggerCustom(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.OperationFilter<AddCorrelationHeaderOperationFilter>();
        });
    }

    public static void UseSwaggerCustom(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
}