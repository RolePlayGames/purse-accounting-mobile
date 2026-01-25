using PurseAccounting.Mobile.Infrastructure.Authorization.MailboxAuthorization;

namespace PurseAccounting.Mobile.Application.Login;

public interface ILoginService
{
    /// <summary>
    /// Logins user by email
    /// </summary>
    /// <param name="login">User email login</param>
    /// <param name="password">User password</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Email authorization result</returns>
    Task<MailboxAuthorizationEnum> Login(string login, string password, CancellationToken cancellationToken);
}
