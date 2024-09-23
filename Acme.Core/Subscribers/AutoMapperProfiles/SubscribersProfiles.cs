using Acme.Core.Subscribers.Dtos;
using Acme.Data.Models;
using AutoMapper;

namespace Acme.Core.Subscribers.AutoMapperProfiles;

public class SubscribersProfiles : Profile
{
    public SubscribersProfiles()
    {
        CreateMap<Subscriber, SubscriberDto>();
    }
}
