using Acme.Core.Emails.Dto;
using Acme.Core.Emails.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Acme.Core.Emails;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddEmailSenderServices(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var emailOptions = ConfigEmailOptions(serviceCollection, configuration);

        if (emailOptions.Enabled)
        {
            serviceCollection.AddScoped<IEmailsSender, EmailSender>();
        }
        else
        {
            serviceCollection.AddScoped<IEmailsSender, DevelopmentEmailSender>();
        }

        return serviceCollection;
    }

    private static EmailOptions ConfigEmailOptions(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var configurationSection = configuration.GetSection("Email");
        var emailOptions = configurationSection.Get<EmailOptions>();
        if (emailOptions is null)
        {
            throw new ArgumentException("EmailOptions is empty");
        }

        serviceCollection.Configure<EmailOptions>(configurationSection);
        return emailOptions;
    }
}
