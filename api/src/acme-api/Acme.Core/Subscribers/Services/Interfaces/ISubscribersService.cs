using Acme.Core.Subscribers.Dtos;

namespace Acme.Core.Subscribers.Services.Interfaces;

public interface ISubscribersService
{
    Task<ICollection<SubscriberDto>> GetAllSubscribers(GetSubscribersFilterDto filters);
}
