namespace AE3803Notification.Extensions;

using Infrastructure.Services;
using Frieren_Guard.Extensions;

public static class ConfigureServices
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddFrierenGuardServices(configuration);
        services.AddWorkers(configuration);
        services.AddInfrastructure(configuration);
    }
}
