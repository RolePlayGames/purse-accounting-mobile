using System.Net.Http.Json;

namespace PurseAccounting.Mobile.Infrastructure.Accounting.TransactionCategories;

internal class TransactionCategoryClient : ITransactionCategoryClient
{
    private readonly HttpClient _httpClient;

    public TransactionCategoryClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyCollection<TransactionCategoryDto>> Get(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<IReadOnlyCollection<TransactionCategoryDto>>("api/accounting/transaction-categories", cancellationToken) ?? [];
        }
        catch (Exception)
        {
            // TODO: log exception
        }

        return [];
    }
}
