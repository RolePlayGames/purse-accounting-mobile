using PurseAccounting.Mobile.Infrastructure.ApiResults.Generics;
using PurseAccounting.Mobile.Infrastructure.Base;
using PurseAccounting.Mobile.Infrastructure.Transactions;
using System.Net.Http.Json;

namespace PurseAccounting.Mobile.Infrastructure.PlannedTransactions.Settings;

internal class PlannedTransactionSettingsClient : ClientBase, IPlannedTransactionSettingsClient
{
    private readonly HttpClient _httpClient;

    public PlannedTransactionSettingsClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<ApiResult<CreatePlannedTransactionSettingResponse>> Create(CreatePlannedTransactionSettingRequest request, CancellationToken cancellationToken)
    {
        return SafeCall<CreatePlannedTransactionSettingResponse>(_httpClient.PutAsJsonAsync, "api/accounting/planned-transaction-settings", request, cancellationToken);
    }

    public Task<ApiResult<AccountAmounts>> Deactivate(long settingID, CancellationToken cancellationToken)
    {
        return SafeCall<AccountAmounts>((url, token) => _httpClient.DeleteAsync(url, token), $"api/accounting/planned-transaction-settings/{settingID}", cancellationToken);
    }

    public Task<ApiResult<IReadOnlyCollection<PlannedTransactionSettingInfo>>> GetInfo(CancellationToken cancellationToken)
    {
        return SafeCall<IReadOnlyCollection<PlannedTransactionSettingInfo>>(_httpClient.GetAsync, "api/accounting/planned-transaction-settings", cancellationToken);
    }

    public Task<ApiResult<AccountAmounts>> Update(long settingID, PlannedTransactionSettingInfo info, CancellationToken cancellationToken)
    {
        return SafeCall<AccountAmounts>(_httpClient.PostAsJsonAsync, $"api/accounting/planned-transaction-settings/{settingID}", info, cancellationToken);
    }
}
