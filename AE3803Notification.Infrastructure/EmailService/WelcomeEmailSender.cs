namespace AE3803Notification.Infrastructure.EmailService;

using Options;
using Domain.User;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

public class WelcomeEmailSender(SendGridClient client, IOptions<EmailTemplates> emailTemplates, IOptions<SenderOptions> senderOptions, ILogger<WelcomeEmailSender> logger) : IEmailSender<WelcomeEmailDto>
{
    private readonly SenderOptions _senderOptions = senderOptions.Value;
    private readonly EmailTemplates _emailTemplates = emailTemplates.Value;

    public async Task SendAsync(WelcomeEmailDto message, string recipient)
    {
        logger.LogInformation("Sending Welcome Email");
        var from = new EmailAddress(_senderOptions.SenderEmail, _senderOptions.SenderName);
        var to = new EmailAddress(recipient);
        var msg = MailHelper.CreateSingleTemplateEmail(from, to, _emailTemplates.WelcomeEmailId, message);
        var response = await client.SendEmailAsync(msg);
        logger.LogInformation("Email sent with status code {statusCode}", response.StatusCode);
    }
}
