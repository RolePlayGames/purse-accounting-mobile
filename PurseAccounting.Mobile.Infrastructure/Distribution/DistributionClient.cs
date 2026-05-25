using PurseAccounting.Mobile.Infrastructure.ApiResults.Generics;
using PurseAccounting.Mobile.Infrastructure.Base;

namespace PurseAccounting.Mobile.Infrastructure.Distribution;

internal class DistributionClient : ClientBase, IDistributionClient
{
    private readonly HttpClient _httpClient;

    public DistributionClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<ApiResult<DistributionStrategyInfo>> GetDistributionStrategy(CancellationToken cancellationToken)
    {
        return SafeCall<DistributionStrategyInfo>(_httpClient.GetAsync, "api/accounting/account/distribution-strategy", cancellationToken);
    }

    public Task<ApiResult<DistributeAccountResponse>> DistributeAllToToday(CancellationToken cancellationToken)
    {
        return SafeCall<DistributeAccountResponse, DistributionExceptionCode>((url, token) => _httpClient.PostAsync(url, null, token), "api/accounting/account/distribute-all-to-today", cancellationToken);
    }

    public Task<ApiResult<DistributeAccountResponse>> DistributeAutomatically(CancellationToken cancellationToken)
    {
        return SafeCall<DistributeAccountResponse, DistributionExceptionCode>((url, token) => _httpClient.PostAsync(url, null, token), "api/accounting/account/distribute-automatically", cancellationToken);
    }

    public Task<ApiResult<DistributeAccountResponse>> DistributeBetweenDays(CancellationToken cancellationToken)
    {
        return SafeCall<DistributeAccountResponse, DistributionExceptionCode>((url, token) => _httpClient.PostAsync(url, null, token), "api/accounting/account/distribute-between-days", cancellationToken);
    }
}
