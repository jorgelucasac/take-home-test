using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fundo.Infrastructure.SqlServer.DependencyInjections
{
    public static class SqlServerDependencyInjection
    {
        public static IServiceCollection AddSqlServerInfrastructure(this IServiceCollection services, IConfiguration configuration, bool isDevelopment = false)
        {
            ConfigureSqlServerDbContext(services, configuration, isDevelopment);
            return services;
        }

        private static void ConfigureSqlServerDbContext(IServiceCollection services, IConfiguration configuration, bool isDevelopment)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString, sql =>
                {
                    sql.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
                    sql.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);

                    options.EnableDetailedErrors(isDevelopment);
                    options.EnableSensitiveDataLogging(isDevelopment);
                }));
        }
    }
}