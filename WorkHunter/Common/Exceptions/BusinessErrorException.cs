namespace Common.Exceptions;

public sealed class BusinessErrorException : Exception
{
    public BusinessErrorException() { }

    public BusinessErrorException(string message) : base(message) { }

    public BusinessErrorException(string message, Exception innerException) : base(message, innerException) { }
}