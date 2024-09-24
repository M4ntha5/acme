using Acme.Core.Subscribers.Dtos;
using Acme.Core.Subscribers.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Acme.Api.Controllers;

[Route("api/[controller]")]
public class SubscribersController(ISubscribersService subscribersService) : Controller
{
    [HttpGet]
    public async Task<ICollection<SubscriberDto>> GetAll( [FromQuery] GetSubscribersFilterDto filters)
    {
        return await subscribersService.GetAllSubscribers(filters);
    }
}
