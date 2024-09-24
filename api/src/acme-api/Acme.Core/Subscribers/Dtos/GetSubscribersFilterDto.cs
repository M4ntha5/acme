
namespace Acme.Core.Subscribers.Dtos;

public record GetSubscribersFilterDto
{
    public string? Email { get; set; }
    public DateOnly? ExpirationDateFrom { get; set; }
    public DateOnly? ExpirationDateTo { get; set; }
}
