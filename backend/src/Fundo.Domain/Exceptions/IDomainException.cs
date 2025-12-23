namespace Fundo.Domain.Exceptions;

public interface IDomainException
{
    public string Message { get; }
    public Exception? InnerException { get; }
    public string? StackTrace { get; }
    public string? Source { get; }

    public string ToString();
}
