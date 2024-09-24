using Acme.Core.Subscribers.Dtos;
using Acme.Core.Subscribers.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Acme.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubscribersController(ISubscribersService subscribersService) : Controller
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ICollection<SubscriberDto>>> GetAll( [FromQuery] GetSubscribersFilterDto filters)
    {
        // TODO use Result pattern in a future
        try
        {
            var subscribers = await subscribersService.GetAllSubscribers(filters);
            
            return !subscribers.Any() ? NoContent() : Ok(subscribers);
        }
        catch (Exception ex)
        {
            return Problem($"Error fetching subscribers data. {ex.Message}");
        }
    }
}
