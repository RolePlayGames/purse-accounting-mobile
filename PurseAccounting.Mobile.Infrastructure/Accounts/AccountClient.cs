using PurseAccounting.Mobile.Infrastructure.ApiResults;
using PurseAccounting.Mobile.Infrastructure.ApiResults.Generics;
using PurseAccounting.Mobile.Infrastructure.Base;
using PurseAccounting.Mobile.Infrastructure.Transactions;
using System.Net.Http.Json;

namespace PurseAccounting.Mobile.Infrastructure.Accounts;

internal class AccountClient : ClientBase, IAccountClient
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

    public Task<ApiResult<AccountAmounts>> UpdateAccount(UpdateAccountRequest request, CancellationToken ct)
    {
        return SafeCall<AccountAmounts>(_httpClient.PutAsJsonAsync, "api/accounting/account", request, ct);
    }
}
