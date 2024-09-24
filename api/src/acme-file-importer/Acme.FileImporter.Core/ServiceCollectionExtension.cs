using Acme.FileImporter.Core.Services;
using Acme.FileImporter.Core.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Acme.FileImporter.Core;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddCoreServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ISubscribersImporter, SubscribersImporter>();
        return serviceCollection;

    }
}
