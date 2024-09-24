using System.Text.RegularExpressions;
using Acme.Data.Contexts;
using Acme.Data.Models;
using Acme.FileImporter.Core.Dtos;
using Acme.FileImporter.Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Acme.FileImporter.Core.Services;

public class SubscribersImporter(ApplicationDbContext context) : ISubscribersImporter
{
    private const int ExpectedColumnCount = 2;
    
    private const int EmailColumnIndex = 0;
    private const int ExpirationDateColumnIndex = 1;
    
    private static readonly Regex EmailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public async Task<FileImportResponseDto> ImportSubscribers(ICollection<FileModel> files)
    {
        var errors = new List<string>();
        var expiredSubscribers = new List<Subscriber>();
        
        foreach (var file in files)
        {
            var fileImportResponse = await ReadFile(file);
            errors.AddRange(fileImportResponse.Errors);
            expiredSubscribers.AddRange(fileImportResponse.ExpiredSubscribers);
        }

        return new(errors, expiredSubscribers.Select(s => new SubscriberDto(s.Id, s.Email, s.ExpirationDate)).ToList());
    }

    private async Task<(ICollection<string> Errors, ICollection<Subscriber> ExpiredSubscribers)> ReadFile(FileModel file)
    {
        var subscribers = new List<Subscriber>();
        var errors = new List<string>();
        var expiredSubscribers = new List<Subscriber>();

        if (file.ContentType != "text/csv")
        {
            errors.Add("Invalid file type. Ony .csv files accepted");
            return new(errors, []);
        }

        if (file.Content is null || file.Content.Length is 0)
        {
            errors.Add($"File is empty File: {file.Name}.");
            return new(errors, []);
        }

        using (var memoryStream = new MemoryStream(file.Content))
        {
            using (var reader = new StreamReader(memoryStream))
            {
                var lineNo = 0;
                
                while (!reader.EndOfStream)
                {
                    ++lineNo;
                    var line = await reader.ReadLineAsync();
                    if (string.IsNullOrEmpty(line))
                    {
                        errors.Add($"Line is empty. File: {file.Name}. Line no: {lineNo}");
                        continue;
                    }
                    var columns = line.Split(',');

                    if (columns.Length != ExpectedColumnCount)
                    {
                        errors.Add($"Line expecting to contain 2 columns. File: {file.Name}. Line no: {lineNo}");
                        continue;
                    }
                    var email = columns[EmailColumnIndex];
                    if (!IsValidEmail(email))
                    {
                        errors.Add($"Invalid email. File: {file.Name}. Line no: {lineNo}");
                        continue;
                    }
            
                    var isDateParsedSuccessfully = DateOnly.TryParse(columns[ExpirationDateColumnIndex], out var expirationDate);
                    if (!isDateParsedSuccessfully)
                    {
                        errors.Add($"Invalid expiration date. File: {file.Name}. Line: {lineNo}");
                        continue;
                    }

                    var today = DateOnly.FromDateTime(DateTime.Now);
                    if (expirationDate < today)
                    {
                        expiredSubscribers.Add(new(email, expirationDate));
                    }
                    else
                    {
                        subscribers.Add(new(email, expirationDate));
                    }
                }
            }
        }
        
        await SaveSubscribersToDb(subscribers);

        return new(errors, expiredSubscribers);
    }

    private async Task SaveSubscribersToDb(ICollection<Subscriber> subscribers)
    {
        var emailsToInsert = subscribers.Select(s => s.Email).ToList();

        var subscribersToSkip = await context.Subscribers
            .Where(s => emailsToInsert.Contains(s.Email))
            .ToListAsync();

        var subscribersToInsert = subscribers
            .Where(s => !subscribersToSkip.Select(ss => s.Email).Contains(s.Email))
            .ToList();
        if (!subscribersToInsert.Any())
        {
            return;
        }

        await context.Subscribers.AddRangeAsync(subscribersToInsert);
        await context.SaveChangesAsync();
    }
    
    private static bool IsValidEmail(string email)
    {
        return EmailRegex.IsMatch(email);
    }
}

