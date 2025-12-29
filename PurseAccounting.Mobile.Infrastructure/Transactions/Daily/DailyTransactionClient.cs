using PurseAccounting.Mobile.Infrastructure.ApiResults;
using System.Net.Http.Json;

namespace PurseAccounting.Mobile.Infrastructure.Transactions.Daily;

internal class DailyTransactionClient : IDailyTransactionClient
{
    private readonly HttpClient _httpClient;

    public DailyTransactionClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<ApiResult<AddTransactionResponse>> AddDailyIncomeTransaction(AddTransactionRequest request, CancellationToken cancellationToken)
    {
        return SafeCallAsync<AddTransactionResponse>(() => _httpClient.PutAsJsonAsync("api/accounting/transactions/daily-transactions/income-transactions", request, cancellationToken), cancellationToken);
    }

    public Task<ApiResult<AddTransactionResponse>> AddDailyWithdrawalTransaction(AddTransactionRequest request, CancellationToken cancellationToken)
    {
        return SafeCallAsync<AddTransactionResponse>(() => _httpClient.PutAsJsonAsync("api/accounting/transactions/daily-transactions/withdrawal-transactions", request, cancellationToken), cancellationToken);
    }

    private async Task<ApiResult<T>> SafeCallAsync<T>(Func<Task<HttpResponseMessage>> call, CancellationToken cancellationToken)
    {
        try
        {
            var response = await call().ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var value = await response.Content.ReadFromJsonAsync<T>(cancellationToken: cancellationToken).ConfigureAwait(false);

                if (value is null)
                    return ApiResult<T>.Failure(new InvalidOperationException($"Http response of type {nameof(T)} deserialized to null."));

                return ApiResult<T>.Success(value);
            }
            else
            {
                string? errorBody = null;

                try
                {
                    errorBody = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    return ApiResult<T>.Failure(ex);
                }

                var message = $"Ошибка сервера: {(int)response.StatusCode}";

                if (!string.IsNullOrWhiteSpace(errorBody))
                    message += $" — {errorBody[..Math.Min(100, errorBody.Length)]}...";

                return ApiResult<T>.Failure(new HttpRequestException(message));
            }
        }
        catch (Exception ex)
        {
            return ApiResult<T>.Failure(ex);
        }
    }
}
