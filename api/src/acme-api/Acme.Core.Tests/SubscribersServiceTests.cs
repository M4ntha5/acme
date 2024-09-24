using Acme.Core.Subscribers.Dtos;
using Acme.Core.Subscribers.Services.Interfaces;
using Acme.Core.Tests.Fixtures;
using Acme.Data.Contexts;
using Acme.Data.Models;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Acme.Core.Tests;

public class SubscribersServiceTests : IClassFixture<BaseApplicationFixture>, IDisposable
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly ISubscribersService _subscribersService;

    public SubscribersServiceTests(BaseApplicationFixture baseFixture)
    {
        var serviceProvider = baseFixture.BuildAndGetServiceProvider();

        _applicationDbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
        _subscribersService = serviceProvider.GetRequiredService<ISubscribersService>();

        _applicationDbContext.Database.BeginTransaction();
    }

    public void Dispose()
    {
        _applicationDbContext.Database.RollbackTransaction();
    }

    [Fact]
    public Task InstanceRegistered()
    {
        Assert.NotNull(_subscribersService);
        return Task.CompletedTask;
    }
    
    
    private async Task SeedSubscribers(int count)
    {
        var subscribers = new List<Subscriber>();
        for (var i = 0; i < count; i++)
        {
            if (i % 2 == 0)
            {
                subscribers.Add(new($"test{i}@test.com", DateOnly.FromDateTime(DateTime.Now.AddDays(i))));
            }
            else
            {
                subscribers.Add(new($"test{i}@test.com", DateOnly.FromDateTime(DateTime.Now.AddDays(i * -1))));
            }
        }

        await _applicationDbContext.Subscribers.AddRangeAsync(subscribers);
        await _applicationDbContext.SaveChangesAsync();
    }

    [Fact]
    public async Task GetAllSubscribers_ShouldReturns5Subscribers_WhenNoFiltersActive()
    {
        const int count = 5;
        await SeedSubscribers(count);
        
        var filters = new GetSubscribersFilterDto();

        var subscribers = await _subscribersService.GetAllSubscribers(filters);

        Assert.Equal(count, subscribers.Count);
    }
    
    [Fact]
    public async Task GetAllSubscribers_ShouldReturns1Subscriber_WhenEmailFilterActive()
    {
        await SeedSubscribers(3);
        
        var filters = new GetSubscribersFilterDto()
        {
            Email = "test1@test.com",
        };

        var subscribers = await _subscribersService.GetAllSubscribers(filters);

        Assert.Single(subscribers);
    }
    
    [Fact]
    public async Task GetAllSubscribers_ShouldReturns2Subscribers_WhenRangeFilterActive()
    {
        await SeedSubscribers(3);
        
        var filters = new GetSubscribersFilterDto()
        {
            ExpirationDateFrom = DateOnly.FromDateTime(DateTime.Now.AddDays(-5)),
            ExpirationDateTo = DateOnly.FromDateTime(DateTime.Now),
        };

        var subscribers = await _subscribersService.GetAllSubscribers(filters);

        Assert.Equal(2, subscribers.Count);
    }
}