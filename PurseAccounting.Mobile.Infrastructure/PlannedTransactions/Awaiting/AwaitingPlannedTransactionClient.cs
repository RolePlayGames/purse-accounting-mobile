using PurseAccounting.Mobile.Infrastructure.ApiResults.Generics;
using PurseAccounting.Mobile.Infrastructure.Base;
using PurseAccounting.Mobile.Infrastructure.Transactions;
using System.Net.Http.Json;

namespace PurseAccounting.Mobile.Infrastructure.PlannedTransactions.Awaiting;

internal class AwaitingPlannedTransactionClient : ClientBase, IAwaitingPlannedTransactionClient
{
    private readonly HttpClient _httpClient;

    public AwaitingPlannedTransactionClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<ApiResult<IReadOnlyCollection<AwaitingPlannedTransactionInfo>>> GetAwaitingPlannedTransactions(CancellationToken cancellationToken)
    {
        return SafeCall<IReadOnlyCollection<AwaitingPlannedTransactionInfo>>(_httpClient.GetAsync, "api/accounting/awaiting-planned-transactions", cancellationToken);
    }

    public Task<ApiResult<AccountAmounts>> ChangeAmount(ChangeAwaitingPlannedTransactionAmountRequest request, CancellationToken cancellationToken)
    {
        return SafeCall<AccountAmounts, ChangeAmountExceptionCode>(_httpClient.PatchAsJsonAsync, "api/accounting/awaiting-planned-transactions", request, cancellationToken);
    }

    public Task<ApiResult<AccountAmounts>> ApplyTransaction(long awaitingPlannedTransactionId, CancellationToken cancellationToken)
    {
        return SafeCall<AccountAmounts, MakeAwaitingPlannedTransactionExceptionCode>((url, token) => _httpClient.PostAsync(url, null, token), $"api/accounting/awaiting-planned-transactions/{awaitingPlannedTransactionId}/apply", cancellationToken);
    }

    public Task<ApiResult<AccountAmounts>> DeclineTransaction(long awaitingPlannedTransactionId, CancellationToken cancellationToken)
    {
        return SafeCall<AccountAmounts, MakeAwaitingPlannedTransactionExceptionCode>((url, token) => _httpClient.PostAsync(url, null, token), $"api/accounting/awaiting-planned-transactions/{awaitingPlannedTransactionId}/decline", cancellationToken);
    }
}
