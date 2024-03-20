namespace AE3803Notification.Infrastructure.EmailService;

using AE3803Notification.Infrastructure.EmailService.Options;
using Domain.User;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

public class UserTransferToExternalOperatorSender(SendGridClient client, IOptions<EmailTemplates> emailTemplates, IOptions<SenderOptions> senderOptions, ILogger<UserTransferToExternalOperatorSender> logger) : IEmailSender<UserTransferCompleteDto>
{
    private readonly SenderOptions _senderOptions = senderOptions.Value;
    private readonly EmailTemplates _emailTemplates = emailTemplates.Value;
    public async Task SendAsync(UserTransferCompleteDto message, string recipient)
    {
        logger.LogInformation("Sending Other provider Email");
        var from = new EmailAddress(_senderOptions.SenderEmail, _senderOptions.SenderName);
        var to = new EmailAddress(recipient);
        var msg = MailHelper.CreateSingleTemplateEmail(from, to, _emailTemplates.UserTransferToExternalProvider, message);
        var response = await client.SendEmailAsync(msg);
        logger.LogInformation("Email sent with status code {statusCode}", response.StatusCode);
    }
}
