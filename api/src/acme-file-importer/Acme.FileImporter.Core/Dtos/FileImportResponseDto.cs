namespace Acme.FileImporter.Core.Dtos;

public record FileImportResponseDto(ICollection<string> Errors, ICollection<SubscriberDto> ExpiredSubscribers);
