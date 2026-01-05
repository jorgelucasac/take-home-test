using Serilog;

namespace Fundo.WebApi.Extensions;

public static class SerilogExtensions
{
    public static void AddSerilog(this IHostBuilder host)
    {
        host.UseSerilog((context, services, configuration) =>
        {
            configuration
                .ReadFrom.Configuration(context.Configuration)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Application", "Fundo.WebApi")
                .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName);
        });
    }
}