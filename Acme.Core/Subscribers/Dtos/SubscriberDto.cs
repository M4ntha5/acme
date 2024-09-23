namespace Acme.Core.Subscribers.Dtos;

public record SubscriberDto(Guid Id, string Email, DateOnly ExpirationDate);
