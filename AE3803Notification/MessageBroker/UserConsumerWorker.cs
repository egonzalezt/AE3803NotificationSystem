namespace AE3803Notification.MessageBroker;

using Frieren_Guard;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using Options;
using Extensions;
using Domain.User;
using Exceptions;
using AE3803Notification.Infrastructure.EmailService;

internal class UserConsumerWorker(
    ILogger<UserConsumerWorker> logger,
    IServiceProvider serviceProvider,
    ConnectionFactory rabbitConnection,
    IHealthCheckNotifier healthCheckNotifier,
    SystemStatusMonitor statusMonitor,
    IOptions<ConsumerConfiguration> queues
    ) : BaseRabbitMQWorker(logger, rabbitConnection.CreateConnection(), healthCheckNotifier, statusMonitor, queues.Value.UserNotificationsQueue)
{
    protected override async Task ProcessMessageAsync(BasicDeliverEventArgs eventArgs, IModel channel, CancellationToken stoppingToken)
    {
        var body = eventArgs.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        var headers = eventArgs.BasicProperties.Headers;
        var operation = headers.GetUserEventType();
        using var scope = serviceProvider.CreateScope();

        if (operation is UserOperations.FirstSignIn)
        {
            var userDto = JsonSerializer.Deserialize<UserSignInUriDto>(message) ?? throw new InvalidBodyException();
            logger.LogInformation("Processing User Welcome request");
            var welcomeEmailDto = new WelcomeEmailDto { passwordUrl = userDto.PasswordSetUri };
            var welcomeEmailUseCase = scope.ServiceProvider.GetRequiredService<IEmailSender<WelcomeEmailDto>>();
            await welcomeEmailUseCase.SendAsync(welcomeEmailDto, userDto.Email);
            var activateEmailDto = new ActivateEmailDto { activateEmailUrl = userDto.VerificationEmailUri };
            var activateEmailUseCase = scope.ServiceProvider.GetRequiredService<IEmailSender<ActivateEmailDto>>();
            await activateEmailUseCase.SendAsync(activateEmailDto, userDto.Email);
            channel.BasicAck(eventArgs.DeliveryTag, false);
        }
        if (operation is UserOperations.ExistsOnOtherProvider)
        {
            var userDto = JsonSerializer.Deserialize<UserOnOtherProvider>(message) ?? throw new InvalidBodyException();
            logger.LogInformation("Processing User on other provider");
            var userOnOtherProviderDto = new UserOnOtherProviderEmailDto { Name = userDto.Name };
            var userOnOtherProviderUseCase = scope.ServiceProvider.GetRequiredService<IEmailSender<UserOnOtherProviderEmailDto>>();
            await userOnOtherProviderUseCase.SendAsync(userOnOtherProviderDto, userDto.Email);
            channel.BasicAck(eventArgs.DeliveryTag, false);
        }
        if (operation is UserOperations.TransferUser)
        {
            var userTransferDto = JsonSerializer.Deserialize<UserTransferCompleteDto>(message) ?? throw new InvalidBodyException();
            logger.LogInformation("Processing User on other provider");
            var userOnOtherProviderUseCase = scope.ServiceProvider.GetRequiredService<IEmailSender<UserTransferCompleteDto>>();
            await userOnOtherProviderUseCase.SendAsync(userTransferDto, userTransferDto.Email);
            channel.BasicAck(eventArgs.DeliveryTag, false);
        }
    }
}
