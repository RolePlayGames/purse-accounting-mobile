using PurseAccounting.Mobile.Infrastructure.ApiResults;
using PurseAccounting.Mobile.Infrastructure.ApiResults.Generics;
using PurseAccounting.Mobile.Infrastructure.Transactions;

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
    /// <returns>Updated account amounts</returns>
    Task<ApiResult<AccountAmounts>> UpdateAccount(UpdateAccountRequest request, CancellationToken ct);
}
