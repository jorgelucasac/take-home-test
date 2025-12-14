namespace Fundo.WebApi.Extensions;

public static class CorsExtensions
{
    public static void AddCorsPolicy(this IServiceCollection services, string policyName)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(policyName, policy =>
            {
                policy
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
    }
}