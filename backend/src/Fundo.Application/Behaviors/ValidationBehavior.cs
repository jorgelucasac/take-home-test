using FluentValidation;
using Fundo.Application.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fundo.Application.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    private readonly ILogger<ValidationBehavior<TRequest, TResponse>> _logger;

    public ValidationBehavior(
        IEnumerable<IValidator<TRequest>> validators,
        ILogger<ValidationBehavior<TRequest, TResponse>> logger)
    {
        _validators = validators;
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        _logger.LogInformation("Start request: {Name}", requestName);
        if (!_validators.Any() || !IsResultOfT(typeof(TResponse)))
        {
            var response = await next();
            _logger.LogInformation("Finish Request: {Name}", requestName);
            return response;
        }

        var context = new ValidationContext<TRequest>(request);

        var results = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = results
            .SelectMany(r => r.Errors)
            .Where(f => f is not null)
            .ToList();

        if (failures.Count == 0)
            return await next();

        var details = failures
            .Select(f => new KeyValuePair<string, string>(f.PropertyName, f.ErrorMessage))
            .ToList();

        var error = Error.Validation(
            message: "One or more validation errors occurred.",
            details: details);

        return CreateFailureResultUsingCtor<TResponse>(error, requestName);
    }

    private static bool IsResultOfT(Type responseType)
        => responseType.IsGenericType && responseType.GetGenericTypeDefinition() == typeof(Result<>);

    private TResponse CreateFailureResultUsingCtor<TResponse>(Error error, string requestName)
    {
        var responseType = typeof(TResponse);

        var ctor = responseType.GetConstructor([typeof(bool), typeof(Error)]);

        if (ctor is null) throw new InvalidOperationException($"Constructor {responseType.Name}(bool, Error) not found.");

        _logger.LogWarning("Finish Request with Validation Errors: {Name} - {@Error}", requestName, error);
        return (TResponse)ctor.Invoke([false, error])!;
    }
}