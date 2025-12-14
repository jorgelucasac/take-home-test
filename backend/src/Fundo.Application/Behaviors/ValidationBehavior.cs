using FluentValidation;
using Fundo.Application.Results;
using MediatR;

namespace Fundo.Application.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        => _validators = validators;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any() || !IsResultOfT(typeof(TResponse)))
            return await next();

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

        return CreateFailureResultUsingCtor<TResponse>(error);
    }

    private static bool IsResultOfT(Type responseType)
        => responseType.IsGenericType && responseType.GetGenericTypeDefinition() == typeof(Result<>);

    private static TResponse CreateFailureResultUsingCtor<TResponse>(Error error)
    {
        var responseType = typeof(TResponse);

        var ctor = responseType.GetConstructor([typeof(bool), typeof(Error)]);

        if (ctor is null) throw new InvalidOperationException($"Constructor {responseType.Name}(bool, Error) not found.");

        return (TResponse)ctor.Invoke([false, error])!;
    }
}