namespace AE3803Notification.Infrastructure.Services;

using Domain.User;
using Infrastructure.EmailService;
using EmailService.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SendGrid;

public static class InfrastructureServiceCollection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EmailProviderOptions>(configuration.GetSection("EmailProviderOptions"));
        services.Configure<EmailTemplates>(configuration.GetSection("EmailProviderOptions:EmailTemplates"));
        services.Configure<SenderOptions>(configuration.GetSection("EmailProviderOptions:SenderOptions"));

        services.AddSingleton<IEmailSender<WelcomeEmailDto>, WelcomeEmailSender>();
        services.AddSingleton<IEmailSender<ActivateEmailDto>, ActivateEmailSender>();
        services.AddSingleton<IEmailSender<UserOnOtherProviderEmailDto>, UserOnOtherProviderSender>();

        services.AddSingleton<SendGridClient>(sp =>
        {
            var apiKey = configuration["EmailProviderOptions:ApiKey:Value"];
            var client = new SendGridClient(apiKey);
            return client;
        });
    }
}
