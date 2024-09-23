using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Acme.Utilities.BackgrouondJobs;

public static class BackgroundJobsServiceCollection
{
    public static IServiceCollection AddBackgroundJobs(this IServiceCollection serviceCollection, Assembly assembly)
    {
        var baseBackgroundJobType = typeof(BackgroundJob);

        var types = assembly.GetTypes()
            .Where(p => baseBackgroundJobType.IsAssignableFrom(p) && p.IsClass && !p.IsAbstract);

        foreach (var type in types)
        {
            serviceCollection.AddScoped(typeof(IBackgroundJob), type);
        }

        serviceCollection.AddScoped<BackgroundJobScheduler>();

        return serviceCollection;
    }
}