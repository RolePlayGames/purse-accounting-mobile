using PurseAccounting.Mobile.Infrastructure.AuthCookieStorages;

namespace PurseAccounting.Mobile.Infrastructure.HttpClientInitializers;

internal class HttpClientInitializer : IHttpClientInitializer
{
    private readonly HttpClient _httpClient;
    private readonly IAuthCookieStorage _cookieStorage;

    public HttpClientInitializer(HttpClient httpClient, IAuthCookieStorage cookieStorage)
    {
        _httpClient = httpClient;
        _cookieStorage = cookieStorage;
    }

    public async Task Initialize()
    {
        var cookie = await _cookieStorage.GetCookies();

        if (string.IsNullOrEmpty(cookie))
            return;

        _httpClient.DefaultRequestHeaders.Remove("Cookie");
        _httpClient.DefaultRequestHeaders.Add("Cookie", cookie);
    }
}
