using System.Net.Http.Json;

namespace PurseAccounting.Mobile.Infrastructure.Transactions;

internal class TransactionsClient : ITransactionsClient
{
    private static readonly Uri _baseUri = new("api/accounting/transactions", UriKind.Relative);
    private readonly HttpClient _httpClient;

    public TransactionsClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyCollection<TransactionInfo>> GetTransactions(IReadOnlyCollection<long>? categoryIds = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var uri = BuildUriWithCategories(categoryIds);

            var response = await _httpClient.GetAsync(uri, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var transactions = await response.Content.ReadFromJsonAsync<IReadOnlyCollection<TransactionInfo>>(cancellationToken);

                if (transactions is not null)
                    return transactions;
            }
        }
        catch (Exception)
        {
            // TODO: log exception
        }

        return [];
    }

    private static Uri BuildUriWithCategories(IReadOnlyCollection<long>? categoryIds)
    {
        if (categoryIds is null || categoryIds.Count == 0)
            return _baseUri;

        var queryString = string.Join("&", categoryIds.Select(id => $"categoryIDs={id}"));
        return new Uri($"{_baseUri}?{queryString}", UriKind.Relative);
    }
}
