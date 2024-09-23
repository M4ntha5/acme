using Acme.Core.Subscribers.Dtos;
using Acme.Core.Subscribers.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Acme.Api.Controllers;

[Route("api/[controller]")]
public class SubscribersController(ISubscribersService subscribersService) : Controller
{
    private const int DefaultPageSize = 20;
    
    [HttpGet]
    public async Task<ICollection<SubscriberDto>> GetAll( [FromQuery] GetSubscribersFilterDto filters, int offset, int limit = DefaultPageSize)
    {
        return await subscribersService.GetAllSubscribers(offset, limit, filters);
    }
}
