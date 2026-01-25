using PurseAccounting.Mobile.Application.AccountFactories;
using PurseAccounting.Mobile.Application.Context;
using PurseAccounting.Mobile.Infrastructure.Accounting;

namespace PurseAccounting.Mobile.Application.Accounting;

internal class AccountingService : IAccountingService
{
    private readonly IAccountClient _accountClient;
    private readonly IAccountFactory _accountFactory;
    private readonly IApplicationContext _applicationContext;

    public AccountingService(IAccountClient accountClient, IAccountFactory accountFactory, IApplicationContext applicationContext)
    {
        _accountClient = accountClient;
        _accountFactory = accountFactory;
        _applicationContext = applicationContext;
    }

    public async Task<Models.Account?> LoadAccount(CancellationToken cancellationToken)
    {
        var response = await _accountClient.GetAccount(cancellationToken);
        var account = response is not null ? _accountFactory.CreateAccount(response) : null;
        return _applicationContext.Account = account;
    }
}
