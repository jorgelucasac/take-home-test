namespace Fundo.Domain.Exceptions;

public class DomainException : Exception, IDomainException
{
    public DomainException() : base()
    {
    }

    public DomainException(string message)
        : base(message)
    {
    }

    public DomainException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}