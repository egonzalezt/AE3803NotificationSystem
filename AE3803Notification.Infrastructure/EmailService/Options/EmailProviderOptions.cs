namespace AE3803Notification.Infrastructure.EmailService.Options;

public class EmailProviderOptions
{
    public ApiKey ApiKey { get; set; }
    public EmailTemplates EmailTemplates { get; set; }
    public SenderOptions SenderOptions { get; set; }
}
