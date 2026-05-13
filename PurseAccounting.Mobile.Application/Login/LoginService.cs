using PurseAccounting.Mobile.Application.Accounts;
using PurseAccounting.Mobile.Infrastructure.Authorization.MailboxAuthorization;

namespace PurseAccounting.Mobile.Application.Login;

internal class LoginService : ILoginService
{
    private readonly IMailboxAuthorizationClient _mailboxAuthorizationClient;
    private readonly IAccountService _accountingService;

    public LoginService(IMailboxAuthorizationClient mailboxAuthorizationClient, IAccountService accountingService)
    {
        _mailboxAuthorizationClient = mailboxAuthorizationClient;
        _accountingService = accountingService;
    }

    public async Task<MailboxAuthorizationEnum> Login(string login, string password, CancellationToken cancellationToken)
    {
        var loginResult = await _mailboxAuthorizationClient.Login(login, password, cancellationToken);

        if (loginResult == MailboxAuthorizationEnum.Success)
            await _accountingService.LoadAccount(cancellationToken);

        return loginResult;
    }
}
