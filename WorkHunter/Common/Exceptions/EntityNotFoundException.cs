namespace Common.Exceptions;

public sealed class EntityNotFoundException : Exception
{
    public EntityNotFoundException() { }

    public EntityNotFoundException(string message) : base(message) { }

    public EntityNotFoundException(string message, Exception innerException) : base(message, innerException) { }

    public EntityNotFoundException(string name, string id) 
        : base($"Запись: {name} id: {id} не найдена!") 
    {
    }
}
