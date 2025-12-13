using Fundo.Domain.Repositories;
using Fundo.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fundo.Infrastructure.Persistence.DependencyInjections;

public static class PersistenceDependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration, bool isDevelopment = false)
    {
        services.ConfigureSqlServerDbContext(configuration, isDevelopment);
        services.AddRepositories();
        return services;
    }

    private static void ConfigureSqlServerDbContext(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString, sql =>
            {
                sql.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
                sql.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);

                options.EnableDetailedErrors(isDevelopment);
                options.EnableSensitiveDataLogging(isDevelopment);
            }));
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ILoanRepository, LoanRepository>();
    }
}