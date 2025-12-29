using System.Net.Http.Json;

namespace PurseAccounting.Mobile.Infrastructure.Accounting;

internal class AccountClient : IAccountClient
{
    private readonly HttpClient _httpClient;

    public AccountClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<AccountDto?> GetAccount(CancellationToken ct)
    {
        var response = await _httpClient.GetAsync("/api/accounting/account", ct);

        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<AccountDto>(ct);

        return null;
    }
}
