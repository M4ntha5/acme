using Acme.Core.Emails;
using Acme.Core.PhoneMessages;
using Acme.Core.Slack;
using Acme.Core.Subscribers;
using Acme.Core.SubscribersImporter;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Acme.Core;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddCoreServices(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddEmailSenderServices(configuration);
        serviceCollection.AddSlackNotificationsSenderServices(configuration);
        serviceCollection.AddPhoneMessagesSenderServices(configuration);

        serviceCollection.AddSubscribersServices();
        serviceCollection.AddSubscribersImporterServices();
        
        
        return serviceCollection;
    }
}
