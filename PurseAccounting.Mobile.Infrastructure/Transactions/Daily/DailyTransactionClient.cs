using PurseAccounting.Mobile.Infrastructure.ApiResults;
using PurseAccounting.Mobile.Infrastructure.Base;
using System.Net.Http.Json;

namespace PurseAccounting.Mobile.Infrastructure.Transactions.Daily;

internal class DailyTransactionClient : ClientBase, IDailyTransactionClient
{
    private readonly HttpClient _httpClient;

    public DailyTransactionClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<ApiResult<AddTransactionResponse>> AddDailyIncomeTransaction(AddTransactionRequest request, CancellationToken cancellationToken)
    {
        return SafeCall<AddTransactionResponse>(_httpClient.PutAsJsonAsync, "api/accounting/transactions/daily-transactions/income-transactions", request, cancellationToken);
    }

    public Task<ApiResult<AddTransactionResponse>> AddDailyWithdrawalTransaction(AddTransactionRequest request, CancellationToken cancellationToken)
    {
        return SafeCall<AddTransactionResponse>(_httpClient.PutAsJsonAsync, "api/accounting/transactions/daily-transactions/withdrawal-transactions", request, cancellationToken);
    }
}
