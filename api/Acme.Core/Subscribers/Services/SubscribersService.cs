using Acme.Core.Subscribers.Dtos;
using Acme.Core.Subscribers.Services.Interfaces;
using Acme.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Acme.Core.Subscribers.Services;

public class SubscribersService(ApplicationDbContext applicationDbContext) : ISubscribersService
{
    public async Task<ICollection<SubscriberDto>> GetAllSubscribers(int offset, int limit, GetSubscribersFilterDto filters)
    {
        var query = applicationDbContext.Subscribers.AsQueryable();
        
        if (filters.ExpirationDateFrom.HasValue)
        {
            query = query.Where(q => q.ExpirationDate >= filters.ExpirationDateFrom);
        }
        
        if (filters.ExpirationDateTo.HasValue)
        {
            query = query.Where(q => q.ExpirationDate <= filters.ExpirationDateTo);
        }

        if (filters.OrderByDirection is OrderDirection.Ascending)
        {
            query = filters.OrderBy switch
            {
                ReadSubscribersRequestDtoOrderBy.ExpirationDate => query.OrderBy(t => t.ExpirationDate),
                ReadSubscribersRequestDtoOrderBy.Email => query.OrderBy(t => t.Email),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        else if (filters.OrderByDirection is OrderDirection.Descending)
        {
            query = filters.OrderBy switch
            {
                ReadSubscribersRequestDtoOrderBy.ExpirationDate => query.OrderByDescending(t => t.ExpirationDate),
                ReadSubscribersRequestDtoOrderBy.Email => query.OrderByDescending(t => t.Email),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        return await query
            .Skip(offset)
            .Take(limit)
            .Select(s => new SubscriberDto(s.Id, s.Email, s.ExpirationDate))
            .ToListAsync();
    }
}
