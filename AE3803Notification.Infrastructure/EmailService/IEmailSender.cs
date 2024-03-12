namespace AE3803Notification.Infrastructure.EmailService;

public interface IEmailSender<T> where T : class
{
    Task SendAsync(T message, string recipient);
}
