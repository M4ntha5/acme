namespace Acme.FileImporter.Core.Dtos;

public record SubscriberDto(Guid Id, string Email, DateOnly ExpirationDate);

