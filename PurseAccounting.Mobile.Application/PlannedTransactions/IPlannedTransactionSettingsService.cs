using PurseAccounting.Mobile.Infrastructure.PlannedTransactions.Settings;

namespace PurseAccounting.Mobile.Application.PlannedTransactions;

public interface IPlannedTransactionSettingsService
{
    /// <summary>
    /// Creates a new planned transaction setting
    /// </summary>
    /// <param name="request">Request with setting details</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Result of the operation</returns>
    Task<CreatePlannedTransactionSettingResult> Create(CreatePlannedTransactionSettingRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Deactivates a planned transaction setting
    /// </summary>
    /// <param name="settingID">Setting ID to deactivate</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Is operation succeeded</returns>
    Task<bool> Deactivate(long settingID, CancellationToken cancellationToken);

    /// <summary>
    /// Gets all planned transaction settings
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of planned transaction settings</returns>
    Task<IReadOnlyCollection<PlannedTransactionSettingInfo>> GetInfo(CancellationToken cancellationToken);

    /// <summary>
    /// Updates a planned transaction setting
    /// </summary>
    /// <param name="settingID">Setting ID to update</param>
    /// <param name="info">Updated setting information</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Is operation succeeded</returns>
    Task<bool> Update(long settingID, PlannedTransactionSettingInfo info, CancellationToken cancellationToken);
}
