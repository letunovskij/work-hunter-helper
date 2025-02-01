namespace Common.Exceptions;

public sealed class EntityDeletedException : Exception
{
    public EntityDeletedException() { }

    public EntityDeletedException(string message) : base(message) { }

    public EntityDeletedException(string message, Exception innerException) : base(message, innerException) { }

    public EntityDeletedException(string name, string id)
        : base($"Запись: {name} id: {id} удалена!") { }

    public EntityDeletedException(string name, Guid id)
        : base($"Запись: {name} id: {id} удалена!") { }
}
