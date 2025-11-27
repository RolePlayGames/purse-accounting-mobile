using Microsoft.Maui.Storage;

namespace PurseAccounting.Mobile.Infrastructure.AuthCookieStorages;

internal class AuthCookieStorage : IAuthCookieStorage
{
    private const string _cookieKey = "auth_cookie";

    public async Task<string?> GetCookies()
    {
        try
        {
            return await SecureStorage.GetAsync(_cookieKey);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public Task SetCookies(string cookie)
    {
        return SecureStorage.SetAsync(_cookieKey, cookie);
    }

    public void ClearCookies()
    {
        SecureStorage.Remove(_cookieKey);
    }
}
