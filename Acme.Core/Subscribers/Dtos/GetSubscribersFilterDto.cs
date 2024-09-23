
namespace Acme.Core.Subscribers.Dtos;

public record GetSubscribersFilterDto
{
    public string? Email { get; set; }

    public DateOnly? ExpirationDateFrom { get; set; }
    public DateOnly? ExpirationDateTo { get; set; }

    public OrderDirection OrderByDirection { get; set; } = OrderDirection.Ascending;
    public ReadSubscribersRequestDtoOrderBy OrderBy { get; set; }
}


public enum ReadSubscribersRequestDtoOrderBy
{
    Email,
    ExpirationDate
}

public enum OrderDirection
{
    Ascending,
    Descending
}
