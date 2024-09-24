using Acme.Core.Subscribers.Services;
using Acme.Core.Subscribers.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Acme.Core.Subscribers;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddSubscribersServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ISubscribersService, SubscribersService>();
        return serviceCollection;
    }
}
