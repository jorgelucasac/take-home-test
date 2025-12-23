namespace Fundo.Domain.Exceptions;

public class DomainArgumentException : ArgumentException, IDomainException
{
    public DomainArgumentException() : base()
    {
    }

    public DomainArgumentException(string message)
        : base(message)
    {
    }

    public DomainArgumentException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public DomainArgumentException(string message, string paramName)
        : base(message, paramName)
    {
    }
}