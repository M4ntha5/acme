using Acme.Core.Emails.Dto;
using Acme.Core.Emails.Services;
using Acme.Core.PhoneMessages.Dto;
using Acme.Core.PhoneMessages.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Acme.Core.PhoneMessages;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddPhoneMessagesSenderServices(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var emailOptions = ConfigPhoneMessageOptions(serviceCollection, configuration);

        if (emailOptions.Enabled)
        {
            serviceCollection.AddScoped<IPhoneMessagesSender, PhoneMessageSender>();
        }
        else
        {
            serviceCollection.AddScoped<IPhoneMessagesSender, DevelopmentPhoneMessagesSender>();
        }

        return serviceCollection;
    }

    private static PhoneMessagesOptions ConfigPhoneMessageOptions(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var configurationSection = configuration.GetSection("PhoneMessages");
        var phoneMessagesOptions = configurationSection.Get<PhoneMessagesOptions>();
        if (phoneMessagesOptions is null)
        {
            throw new ArgumentException("PhoneMessagesOptions is empty");
        }

        serviceCollection.Configure<PhoneMessagesOptions>(configurationSection);
        return phoneMessagesOptions;
    }
}
