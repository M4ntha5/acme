namespace Acme.Core.Emails.Dto;

public record EmailOptions
{
    public required bool Enabled { get; init; }
}
