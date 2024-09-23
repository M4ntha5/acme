namespace Acme.Core.PhoneMessages.Dto;

public record PhoneMessagesOptions
{
    public required bool Enabled { get; init; }
}
