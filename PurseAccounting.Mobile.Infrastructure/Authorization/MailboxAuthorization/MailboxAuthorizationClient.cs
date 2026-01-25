using PurseAccounting.Mobile.Infrastructure.AuthCookieStorages;
using System.Net.Http.Json;
using System.Text.Json;

namespace PurseAccounting.Mobile.Infrastructure.Authorization.MailboxAuthorization;

internal class MailboxAuthorizationClient : IMailboxAuthorizationClient
{
    private static readonly JsonSerializerOptions _serializerOptions = new() { PropertyNameCaseInsensitive = true };

    private readonly HttpClient _httpClient;
    private readonly IAuthCookieStorage _authCookieStorage;

    public MailboxAuthorizationClient(HttpClient httpClient, IAuthCookieStorage authCookieStorage)
    {
        _httpClient = httpClient;
        _authCookieStorage = authCookieStorage;
    }

    public async Task<MailboxAuthorizationEnum> Login(string login, string password, CancellationToken cancellationToken)
    {
        var request = new LoginRequest { Login = login, Password = password };

        HttpResponseMessage response;

        try
        {
            response = await _httpClient.PostAsJsonAsync("/api/authorization/mailbox/login", request, cancellationToken);
        }
        catch (Exception)
        {
            // TODO: add logs here
            return MailboxAuthorizationEnum.UserNotMatched;
        }

        if (response.IsSuccessStatusCode)
        {
            if (response.Headers.TryGetValues("Set-Cookie", out var cookies))
            {
                var cookieHeader = string.Join("; ", cookies.Select(static x => x.Split(';')[0]));
                await _authCookieStorage.SetCookies(cookieHeader);
            }

            return MailboxAuthorizationEnum.Success;
        }

        if (response.StatusCode == System.Net.HttpStatusCode.UnprocessableEntity)
        {
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var error = JsonSerializer.Deserialize<BaseNotice>(content, _serializerOptions);

            return error?.NoticeType switch
            {
                "UserIsNotConfirmed" => MailboxAuthorizationEnum.UserNotConfirmed,
                _ => MailboxAuthorizationEnum.UserNotMatched,
            };
        }

        return MailboxAuthorizationEnum.UserNotMatched;
    }
}
