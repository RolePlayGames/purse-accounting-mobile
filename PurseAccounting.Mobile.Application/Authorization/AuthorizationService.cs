using PurseAccounting.Mobile.Application.Accounts;
using PurseAccounting.Mobile.Application.Context;
using PurseAccounting.Mobile.Infrastructure.Authorization;
using PurseAccounting.Mobile.Infrastructure.Authorization.MailboxAuthorization;

namespace PurseAccounting.Mobile.Application.Authorization;

internal class AuthorizationService : IAuthorizationService
{
    private readonly IMailboxAuthorizationClient _mailboxAuthorizationClient;
    private readonly IAuthorizationClient _authorizationClient;
    private readonly IAccountService _accountingService;
    private readonly IApplicationContext _applicationContext;

    public AuthorizationService(
        IMailboxAuthorizationClient mailboxAuthorizationClient,
        IAuthorizationClient authorizationClient,
        IAccountService accountingService,
        IApplicationContext applicationContext)
    {
        _mailboxAuthorizationClient = mailboxAuthorizationClient;
        _authorizationClient = authorizationClient;
        _accountingService = accountingService;
        _applicationContext = applicationContext;
    }

    public async Task<MailboxAuthorizationEnum> LoginByEmail(string email, string password, CancellationToken cancellationToken)
    {
        var loginResult = await _mailboxAuthorizationClient.Login(email, password, cancellationToken);

        if (loginResult == MailboxAuthorizationEnum.Success)
            await _accountingService.LoadAccount(cancellationToken);

        return loginResult;
    }

    public async Task<bool> Logout(CancellationToken cancellationToken)
    {
        var isSucceed = await _authorizationClient.Logout(cancellationToken);

        if (isSucceed)
            _applicationContext.Account = null;

        return isSucceed;
    }
}
