using Acme.Core.Emails.Dto;
using Acme.Core.Emails.Services;
using Acme.Core.Slack.Dto;
using Acme.Core.Slack.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Acme.Core.Slack;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddSlackNotificationsSenderServices(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var emailOptions = ConfigSlackOptions(serviceCollection, configuration);

        if (emailOptions.Enabled)
        {
            serviceCollection.AddScoped<ISlackNotificationSender, SlackNotificationSender>();
        }
        else
        {
            serviceCollection.AddScoped<ISlackNotificationSender, DevelopmentSlackNotificationSender>();
        }

        return serviceCollection;
    }

    private static SlackOptions ConfigSlackOptions(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var configurationSection = configuration.GetSection("Slack");
        var slackOptions = configurationSection.Get<SlackOptions>();
        if (slackOptions is null)
        {
            throw new ArgumentException("SlackOptions is empty");
        }

        serviceCollection.Configure<SlackOptions>(configurationSection);
        return slackOptions;
    }
}
