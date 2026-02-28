using PurseAccounting.Mobile.Infrastructure.AuthCookieStorages;
using System.Text.Json;

namespace PurseAccounting.Mobile.Infrastructure.Authorization;

internal class AuthorizationClient : IAuthorizationClient
{
    private readonly HttpClient _httpClient;

    public AuthorizationClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> Logout(CancellationToken cancellationToken)
    {
        try
        {
            var response = await _httpClient.PutAsync("/api/authorization/logout", null, cancellationToken);
            return true;
        }
        catch (Exception)
        {
            // TODO: add logs here
            return false;
        }
    }
}
