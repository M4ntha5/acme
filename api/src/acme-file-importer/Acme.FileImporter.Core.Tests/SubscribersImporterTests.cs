using Acme.Data.Contexts;
using Acme.FileImporter.Core.Dtos;
using Acme.FileImporter.Core.Services.Interfaces;
using Acme.FileImporter.Core.Tests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Acme.FileImporter.Core.Tests;

public class SubscribersImporterTests: IClassFixture<BaseApplicationFixture>, IDisposable
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly ISubscribersImporter _subscribersImporter;

    public SubscribersImporterTests(BaseApplicationFixture baseFixture)
    {
        var serviceProvider = baseFixture.BuildAndGetServiceProvider();

        _applicationDbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
        _subscribersImporter = serviceProvider.GetRequiredService<ISubscribersImporter>();

        _applicationDbContext.Database.BeginTransaction();
    }

    public void Dispose()
    {
        _applicationDbContext.Database.RollbackTransaction();
    }

    [Fact]
    public Task InstanceRegistered()
    {
        Assert.NotNull(_subscribersImporter);
        return Task.CompletedTask;
    }

    [Fact]
    public async Task ImportSubscribers_ShouldReturnError_WhenFileTypeIsInvalid()
    {
        var file = new FileModel
        {
            Name = "subscribers.txt",
            ContentType = "text/plain",
            Content = new byte[1]
        };
        
        var result = await _subscribersImporter.ImportSubscribers(new List<FileModel> { file });
        
        Assert.Single(result.Errors);
        Assert.Equal("Invalid file type. Ony .csv files accepted", result.Errors.First());
    }

    [Fact]
    public async Task ImportSubscribers_ShouldReturnError_WhenFileIsEmpty()
    {
        var file = new FileModel
        {
            Name = "subscribers.csv",
            ContentType = "text/csv",
            Content = Array.Empty<byte>()
        };
        
        var result = await _subscribersImporter.ImportSubscribers(new List<FileModel> { file });
        
        Assert.Single(result.Errors);
        Assert.Equal($"File is empty File: {file.Name}.", result.Errors.First());
    }

    [Fact]
    public async Task ImportSubscribers_ShouldReturnError_WhenLineHasInvalidColumns()
    {
        var fileContent = "email@example.com";
        var file = CreateFileModel(fileContent);
        
        var result = await _subscribersImporter.ImportSubscribers(new List<FileModel> { file });
        
        Assert.Single(result.Errors);
        Assert.Contains("Line expecting to contain 2 columns", result.Errors.First());
    }

    [Fact]
    public async Task ImportSubscribers_ShouldReturnError_WhenEmailIsInvalid()
    {
        var fileContent = "invalidemail,2024-01-01";
        var file = CreateFileModel(fileContent);
        
        var result = await _subscribersImporter.ImportSubscribers(new List<FileModel> { file });

        Assert.Single(result.Errors);
        Assert.Contains("Invalid email", result.Errors.First());
    }

    [Fact]
    public async Task ImportSubscribers_ShouldReturnError_WhenExpirationDateIsInvalid()
    {
        var fileContent = "email@example.com,invalid-date";
        var file = CreateFileModel(fileContent);
        
        var result = await _subscribersImporter.ImportSubscribers(new List<FileModel> { file });
        
        Assert.Single(result.Errors);
        Assert.Contains("Invalid expiration date", result.Errors.First());
    }

    [Fact]
    public async Task ImportSubscribers_ShouldSaveValidSubscribersToDb()
    {
        var fileContent = "email1@example.com,2024-01-01\nemail2@example.com,2025-01-01";
        var file = CreateFileModel(fileContent);
        
        var result = await _subscribersImporter.ImportSubscribers(new List<FileModel> { file });

        var addedSubscribers = await _applicationDbContext.Subscribers.ToListAsync();
        
        Assert.Empty(result.Errors);
        Assert.Single(result.ExpiredSubscribers);
        Assert.Single(addedSubscribers);
    }

    private static FileModel CreateFileModel(string content)
    {
        return new FileModel
        {
            Name = "subscribers.csv",
            ContentType = "text/csv",
            Content = System.Text.Encoding.UTF8.GetBytes(content)
        };
    }
}
