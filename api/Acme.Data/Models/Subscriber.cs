using Acme.Data.Base;

namespace Acme.Data.Models;

public class Subscriber : IdBasedEntity
{
    public string Email { get; set; }
    public DateOnly ExpirationDate { get; set; }

    public Subscriber(string email, DateOnly expirationDate)
    {
        Email = email;
        ExpirationDate = expirationDate;
    }
}
