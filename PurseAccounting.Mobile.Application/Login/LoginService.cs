using PurseAccounting.Mobile.Infrastructure.Authorization.MailboxAuthorization;

namespace PurseAccounting.Mobile.Application.Login
{
    internal class LoginService : ILoginService
    {
        private readonly IMailboxAuthorizationClient _mailboxAuthorizationClient;

        public LoginService(IMailboxAuthorizationClient mailboxAuthorizationClient)
        {
            _mailboxAuthorizationClient = mailboxAuthorizationClient;
        }

        public async Task Login(string login, string password, CancellationToken cancellationToken)
        {
            await _mailboxAuthorizationClient.Login(login, password, cancellationToken);
        }
    }
}
