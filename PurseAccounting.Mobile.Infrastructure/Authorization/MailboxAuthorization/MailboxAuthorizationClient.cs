using System.Net.Http.Json;
using System.Text.Json;

namespace PurseAccounting.Mobile.Infrastructure.Authorization.MailboxAuthorization
{
    internal class MailboxAuthorizationClient : IMailboxAuthorizationClient
    {
        private readonly HttpClient _httpClient;

        public MailboxAuthorizationClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<MailboxAuthorizationEnum> Login(string login, string password, CancellationToken cancellationToken)
        {
            var request = new LoginRequest { Login = login, Password = password };

            var response = await _httpClient.PostAsJsonAsync("/api/authorization/mailbox/login", request, cancellationToken);

            if (response.IsSuccessStatusCode)
                return MailboxAuthorizationEnum.Success;

            if (response.StatusCode == System.Net.HttpStatusCode.UnprocessableEntity)
            {
                var content = await response.Content.ReadAsStringAsync(cancellationToken);
                var error = JsonSerializer.Deserialize<BaseNotice>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

                return error.NoticeType switch
                {
                    "CannotMatchAnyUser" => MailboxAuthorizationEnum.UserNotMatched,
                    "UserIsNotConfirmed" => MailboxAuthorizationEnum.UserNotConfirmed,
                    _ => throw new InvalidOperationException($"Unhandled error response: {error}"),
                };
            }

            throw new HttpRequestException($"Server error: {response.StatusCode}");
        }
    }
}
