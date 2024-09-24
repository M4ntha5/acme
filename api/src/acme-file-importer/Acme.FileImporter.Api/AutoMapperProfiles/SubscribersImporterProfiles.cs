using Acme.Data.Models;
using Acme.FileImporter.Core.Dtos;
using AutoMapper;

namespace Acme.FileImporter.Api.AutoMapperProfiles;

public class SubscribersImporterProfiles : Profile
{
    public SubscribersImporterProfiles()
    {
        CreateMap<Subscriber, SubscriberDto>();
    }
}
