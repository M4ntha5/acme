namespace Acme.Core.PhoneMessages.Services;

public interface IPhoneMessagesSender
{
    Task SendPhoneMessage(string recipient, string message);
}
