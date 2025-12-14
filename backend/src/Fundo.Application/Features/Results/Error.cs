namespace Fundo.Application.Features.Results;

public class Error
{
    public ErrorType Type { get; set; }
    public string Message { get; set; }
    public List<KeyValuePair<string, string>> Details { get; set; } = new();

    private Error(ErrorType type, string message)
    {
        Type = type;
        Message = message;
    }

    private Error(ErrorType type, string message, List<KeyValuePair<string, string>> details)
    {
        Type = type;
        Message = message;
        Details = details;
    }

    public static Error Failure(string message)
    {
        return new Error(ErrorType.Failure, message);
    }

    public static Error Validation(string message, List<KeyValuePair<string, string>> details)
    {
        ArgumentNullException.ThrowIfNull(details);
        return new Error(ErrorType.Validation, message, details);
    }

    public static Error NotFound(string message)
    {
        return new Error(ErrorType.NotFound, message);
    }

    public static Error Create(string message, ErrorType type, List<KeyValuePair<string, string>>? details = null)
    {
        return type switch
        {
            ErrorType.Failure => Failure(message),
            ErrorType.Validation => Validation(message, details),
            ErrorType.NotFound => NotFound(message),
            _ => Failure(message)
        };
    }
}