namespace AE3803Notification.Exceptions;

public class InvalidEventTypeException : Exception
{
    public InvalidEventTypeException() : base()
    {
    }

    public InvalidEventTypeException(string message) : base($"EventType {message} not supported")
    {
    }

    public InvalidEventTypeException(string message, Exception innerException) : base(message, innerException)
    {
    }
}