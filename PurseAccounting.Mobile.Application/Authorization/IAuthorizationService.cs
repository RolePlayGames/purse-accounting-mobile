using PurseAccounting.Mobile.Infrastructure.Authorization.MailboxAuthorization;

namespace PurseAccounting.Mobile.Application.Authorization;

public interface IAuthorizationService
{
    /// <summary>
    /// Logins user by email
    /// </summary>
    /// <param name="email">User email</param>
    /// <param name="password">User password</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Email authorization result</returns>
    Task<MailboxAuthorizationEnum> LoginByEmail(string email, string password, CancellationToken cancellationToken);

    /// <summary>
    /// Logout user
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Is logout succeed</returns>
    Task<bool> Logout(CancellationToken cancellationToken);
}
