namespace AE3803Notification.Infrastructure.EmailService;

using Domain.User;
using Infrastructure.EmailService.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

internal class UserOnOtherProviderSender(SendGridClient client, IOptions<EmailTemplates> emailTemplates, IOptions<SenderOptions> senderOptions, ILogger<UserOnOtherProviderSender> logger) : IEmailSender<UserOnOtherProviderEmailDto>
{
    private readonly SenderOptions _senderOptions = senderOptions.Value;
    private readonly EmailTemplates _emailTemplates = emailTemplates.Value;

    public async Task SendAsync(UserOnOtherProviderEmailDto message, string recipient)
    {
        logger.LogInformation("Sending Other provider Email");
        var from = new EmailAddress(_senderOptions.SenderEmail, _senderOptions.SenderName);
        var to = new EmailAddress(recipient);
        var msg = MailHelper.CreateSingleTemplateEmail(from, to, _emailTemplates.UserOnOtherProviderId, message);
        var response = await client.SendEmailAsync(msg);
        logger.LogInformation("Email sent with status code {statusCode}", response.StatusCode);
    }
}
