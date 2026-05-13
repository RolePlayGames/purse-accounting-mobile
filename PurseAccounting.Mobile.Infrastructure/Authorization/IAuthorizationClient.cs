namespace PurseAccounting.Mobile.Infrastructure.Authorization;

public interface IAuthorizationClient
{
    /// <summary>
    /// Logout user on backend
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Is logout succeed</returns>
    Task<bool> Logout(CancellationToken cancellationToken);
}
