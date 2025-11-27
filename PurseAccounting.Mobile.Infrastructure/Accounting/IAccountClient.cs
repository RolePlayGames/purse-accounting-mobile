namespace PurseAccounting.Mobile.Infrastructure.Accounting;

public interface IAccountClient
{
    /// <summary>
    /// Gets account from server
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Account or null on fail</returns>
    Task<Account?> GetAccount(CancellationToken ct);
}
