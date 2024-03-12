namespace AE3803Notification.Exceptions;

public class HeaderNotFoundException : Exception
{
    public HeaderNotFoundException() : base()
    {
    }

    public HeaderNotFoundException(string message) : base($"Header {message} not found")
    {
    }

    public HeaderNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
