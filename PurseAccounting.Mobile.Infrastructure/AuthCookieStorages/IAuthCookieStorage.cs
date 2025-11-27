namespace PurseAccounting.Mobile.Infrastructure.AuthCookieStorages;

internal interface IAuthCookieStorage
{
    /// <summary>
    /// Gets auth cookies from secure storage
    /// </summary>
    /// <returns>Cookies if exists</returns>
    Task<string?> GetCookies();

    /// <summary>
    /// Sets auth cookies to secure storage
    /// </summary>
    /// <param name="cookie">Cookies</param>
    Task SetCookies(string cookie);

    /// <summary>
    /// Clear auth cookies in secure storage
    /// </summary>
    void ClearCookies();
}
