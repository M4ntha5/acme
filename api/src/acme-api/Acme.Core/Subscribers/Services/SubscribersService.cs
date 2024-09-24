using Acme.Core.Subscribers.Dtos;
using Acme.Core.Subscribers.Services.Interfaces;
using Acme.Data.Contexts;
using Acme.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Acme.Core.Subscribers.Services;

public class SubscribersService(ApplicationDbContext applicationDbContext) : ISubscribersService
{
    public async Task<ICollection<SubscriberDto>> GetAllSubscribers(GetSubscribersFilterDto filters)
    {
        var query = applicationDbContext.Subscribers.AsQueryable();

        query = ApplyFilters(query, filters);
        
       return await query
            .Select(s => new SubscriberDto(s.Id, s.Email, s.ExpirationDate))
            .ToListAsync();
    }

    private static IQueryable<Subscriber> ApplyFilters(IQueryable<Subscriber> query, GetSubscribersFilterDto filters)
    {
        if (!string.IsNullOrEmpty(filters.Email))
        {
            query = query.Where(q => q.Email.Contains(filters.Email));
        }
        if (filters.ExpirationDateFrom.HasValue)
        {
            query = query.Where(q => q.ExpirationDate >= filters.ExpirationDateFrom);
        }
        
        if (filters.ExpirationDateTo.HasValue)
        {
            query = query.Where(q => q.ExpirationDate <= filters.ExpirationDateTo);
        }

        return query;
    }
}
