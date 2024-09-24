using Acme.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Acme.Core.Tests.Fixtures;

public class BaseFixture : IDisposable
{
    private static readonly object Lock = new object();
    private static bool _databaseInitialized;

    protected ServiceCollection ServiceCollection { get; }
    protected IConfiguration Configuration { get; }

    public ServiceProvider BuildAndGetServiceProvider(Action<ServiceCollection> options)
    {
        options.Invoke(ServiceCollection);
        return ServiceCollection.BuildServiceProvider();
    }

    public ServiceProvider BuildAndGetServiceProvider()
    {
        return ServiceCollection.BuildServiceProvider();
    }

    public BaseFixture()
    {
        ServiceCollection = new ServiceCollection();

        Configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>()
            {
                { "Email:Enabled", "false" },
                { "Slack:Enabled", "false" },
                { "PhoneMessages:Enabled", "false" },
            }!)
            .Build();
        
        ServiceCollection.AddLogging();
        ServiceCollection.AddSingleton(Configuration);
        ServiceCollection.AddCoreServices(Configuration);
            
        ServiceCollection.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql("User ID=postgres;Password=postgres;Server=localhost;Port=10432;Database=testing-subscribers;");
        });

        InitializeDatabase();
    }

    private void InitializeDatabase()
    {
        lock (Lock)
        {
            if (_databaseInitialized) return;

            var provider = ServiceCollection.BuildServiceProvider();
            var dbContext = provider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.Migrate();

            _databaseInitialized = true;
        }
    }

    public void Dispose()
    {
    }
}