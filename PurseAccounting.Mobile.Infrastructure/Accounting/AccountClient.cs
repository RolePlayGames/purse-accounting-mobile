using System.Net.Http.Json;

namespace PurseAccounting.Mobile.Infrastructure.Accounting;

internal class AccountClient : IAccountClient
{
    private static readonly string _getAccountUrl = "/api/accounting/account";

    private readonly HttpClient _httpClient;

    public AccountClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Account?> GetAccount(CancellationToken ct)
    {
        var response = await _httpClient.GetAsync(_getAccountUrl, ct);

        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<Account>(ct);

        return null;
    }
}
