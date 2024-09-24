using Acme.Core.Subscribers.Dtos;
using Acme.Core.Subscribers.Services.Interfaces;
using Acme.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Acme.Core.Subscribers.Services;

public class SubscribersService(ApplicationDbContext applicationDbContext) : ISubscribersService
{
    public async Task<ICollection<SubscriberDto>> GetAllSubscribers(GetSubscribersFilterDto filters)
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
        
        return await query
            .Select(s => new SubscriberDto(s.Id, s.Email, s.ExpirationDate))
            .ToListAsync();
    }
}
