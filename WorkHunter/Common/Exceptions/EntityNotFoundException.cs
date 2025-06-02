using DocumentFormat.OpenXml.Wordprocessing;

namespace Common.Exceptions;

public sealed class EntityNotFoundException : Exception
{
    public EntityNotFoundException() { }

    public EntityNotFoundException(string message) : base(message) { }

    public EntityNotFoundException(string message, Exception innerException) : base(message, innerException) { }

    public EntityNotFoundException(string id, string name) 
        : base($"Запись: {name} id: {id} не найдена!") { }

    public EntityNotFoundException(string name, Guid id)
        : base($"Запись: {name} id: {id} не найдена!") { }
}
