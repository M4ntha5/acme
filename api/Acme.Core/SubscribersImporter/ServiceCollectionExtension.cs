using Acme.Core.SubscribersImporter.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Acme.Core.SubscribersImporter;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddSubscribersImporterServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ISubscribersImporter, Services.SubscribersImporter>();
        return serviceCollection;
    }
}
