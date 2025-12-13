namespace Fundo.Application.Handlers.Results
{
    public class Error
    {
        public ErrorType Type { get; set; }
        public string Message { get; set; }
        public List<KeyValuePair<string, string>> Details { get; set; } = new();

        public Error(ErrorType type, string message)
        {
            Type = type;
            Message = message;
        }

        public Error(ErrorType type, string message, List<KeyValuePair<string, string>> details)
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
            return new Error(ErrorType.Validation, message, details);
        }

        public static Error Problem(string message)
        {
            return new Error(ErrorType.Problem, message);
        }

        public static Error NotFound(string message)
        {
            return new Error(ErrorType.NotFound, message);
        }

        public static Error Conflict(string message)
        {
            return new Error(ErrorType.Conflict, message);
        }
    }
}