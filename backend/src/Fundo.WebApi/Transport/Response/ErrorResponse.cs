using Fundo.Application.Results;

namespace Fundo.WebApi.Transport.Response;

public class ErrorResponse
{
    public string Message { get; set; }
    public List<KeyValuePair<string, string>> Details { get; set; } = new();

    public ErrorResponse(string message, List<KeyValuePair<string, string>>? details = null)
    {
        Message = message;
        Details = details ?? [];
    }

    public static ErrorResponse From(Error error)
    {
        return new ErrorResponse(error.Message, error.Details);
    }
}