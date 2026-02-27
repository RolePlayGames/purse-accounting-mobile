using PurseAccounting.Mobile.Infrastructure.ApiResults;

namespace PurseAccounting.Mobile.Infrastructure.Accounts;

public interface IAccountClient
{
    /// <summary>
    /// Gets account from server
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Account or null on fail</returns>
    Task<AccountDto?> GetAccount(CancellationToken ct);

    /// <summary>
    /// Updates account
    /// </summary>
    /// <param name="request">New account data</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Functional result</returns>
    Task<ApiResult> UpdateAccount(UpdateAccountRequest request, CancellationToken ct);
}
