using Acme.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Acme.FileImporter.Core.Tests.Fixtures;

public class BaseFixture : IDisposable
{
    private static readonly object Lock = new object();
    private static bool _databaseInitialized;

    protected ServiceCollection ServiceCollection { get; }

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
        
        ServiceCollection.AddLogging();
        ServiceCollection.AddCoreServices();
            
        ServiceCollection.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql("User ID=postgres;Password=postgres;Server=localhost;Port=10433;Database=testing-subscribers-importer;");
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