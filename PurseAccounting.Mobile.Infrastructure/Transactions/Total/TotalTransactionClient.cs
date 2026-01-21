using PurseAccounting.Mobile.Infrastructure.ApiResults;
using PurseAccounting.Mobile.Infrastructure.Base;
using System.Net.Http.Json;

namespace PurseAccounting.Mobile.Infrastructure.Transactions.Total;

internal class TotalTransactionClient : ClientBase, ITotalTransactionClient
{
    private readonly HttpClient _httpClient;

    public TotalTransactionClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<ApiResult<AddTransactionResponse>> AddTotalIncomeTransaction(AddTransactionRequest request, CancellationToken cancellationToken)
    {
        return SafeCall<AddTransactionResponse, AddTotalTransactoinsExceptionCode>(_httpClient.PutAsJsonAsync, "api/accounting/transactions/total-transactions/income-transactions", request, cancellationToken);
    }

    public Task<ApiResult<AddTransactionResponse>> AddTotalWithdrawalTransaction(AddTransactionRequest request, CancellationToken cancellationToken)
    {
        return SafeCall<AddTransactionResponse, AddTotalTransactoinsExceptionCode>(_httpClient.PutAsJsonAsync, "api/accounting/transactions/total-transactions/withdrawal-transactions", request, cancellationToken);
    }
}
