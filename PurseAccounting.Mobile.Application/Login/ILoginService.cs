using PurseAccounting.Mobile.Infrastructure.Authorization.MailboxAuthorization;

namespace PurseAccounting.Mobile.Application.Login;

public interface ILoginService
{
    Task<MailboxAuthorizationEnum> Login(string login, string password, CancellationToken cancellationToken);
}
