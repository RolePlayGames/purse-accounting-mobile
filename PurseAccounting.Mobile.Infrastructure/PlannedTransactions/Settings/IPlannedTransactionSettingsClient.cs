using PurseAccounting.Mobile.Infrastructure.ApiResults.Generics;
using PurseAccounting.Mobile.Infrastructure.Transactions;

namespace PurseAccounting.Mobile.Infrastructure.PlannedTransactions.Settings;

public interface IPlannedTransactionSettingsClient
{
    /// <summary>
    /// Creates a new planned transaction setting
    /// </summary>
    /// <param name="request">Request with setting details</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Response with created setting ID and account amounts</returns>
    Task<ApiResult<CreatePlannedTransactionSettingResponse>> Create(CreatePlannedTransactionSettingRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Deactivates a planned transaction setting
    /// </summary>
    /// <param name="settingID">Setting ID to deactivate</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>New account amounts</returns>
    Task<ApiResult<AccountAmounts>> Deactivate(long settingID, CancellationToken cancellationToken);

    /// <summary>
    /// Gets all planned transaction settings
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of planned transaction settings</returns>
    Task<ApiResult<IReadOnlyCollection<PlannedTransactionSettingInfo>>> GetInfo(CancellationToken cancellationToken);

    /// <summary>
    /// Updates a planned transaction setting
    /// </summary>
    /// <param name="settingID">Setting ID to update</param>
    /// <param name="info">Updated setting information</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>New account amounts</returns>
    Task<ApiResult<AccountAmounts>> Update(long settingID, PlannedTransactionSettingInfo info, CancellationToken cancellationToken);
}
