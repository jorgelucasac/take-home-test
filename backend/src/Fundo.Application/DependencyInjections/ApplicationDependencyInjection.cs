using FluentValidation;
using Fundo.Application.Handlers.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Fundo.Application.DependencyInjections;

public static class ApplicationDependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(ApplicationDependencyInjection).Assembly;

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
        services.AddValidatorsFromAssembly(assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    }
}