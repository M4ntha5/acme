using Acme.Core.Subscribers.Dtos;

namespace Acme.Core.SubscribersImporter.Dtos;

public record FileImportResponseDto(ICollection<string> Errors, ICollection<SubscriberDto> ExpiredSubscribers);
