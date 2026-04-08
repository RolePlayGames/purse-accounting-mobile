using System.Net.Http.Json;

namespace PurseAccounting.Mobile.Infrastructure.Transactions;

internal class TransactionsClient : ITransactionsClient
{
    private readonly HttpClient _httpClient;
    private static readonly Uri BaseUri = new("api/accounting/transactions", UriKind.Relative);

    public TransactionsClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyCollection<TransactionInfo>> GetTransactions(
        IReadOnlyCollection<long>? categoryIDs = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            Uri uri = BuildUri(categoryIDs);

            var response = await _httpClient.GetAsync(uri, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var transactions = await response.Content.ReadFromJsonAsync<IReadOnlyCollection<TransactionInfo>>(cancellationToken);
                
                if (transactions is not null)
                {
                    return transactions.Select(t => new TransactionInfo(
                        t.ID,
                        t.Amount,
                        t.Date,
                        Enum.Parse<TransactionChangeAmountType>(t.ChangeAmountType).ToString(),
                        t.TransactionCategoryID
                    )).ToList();
                }
            }
        }
        catch (Exception)
        {
            // TODO: log exception
        }

        return [];
    }

    private static Uri BuildUri(IReadOnlyCollection<long>? categoryIDs)
    {
        if (categoryIDs is null || categoryIDs.Count == 0)
        {
            return BaseUri;
        }

        var queryString = string.Join("&", categoryIDs.Select(id => $"categoryIDs={id}"));
        return new Uri($"{BaseUri}?{queryString}", UriKind.Relative);
    }
}
