namespace Fundo.Domain.Entities;

public abstract class BaseEntity<TypeId>
{
    public TypeId Id { get; protected set; }
    public DateTime CreatedAt { get; private set; }

    protected BaseEntity()
    {
        CreatedAt = DateTime.UtcNow;
    }
}