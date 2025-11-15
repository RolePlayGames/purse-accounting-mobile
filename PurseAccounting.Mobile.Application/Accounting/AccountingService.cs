using PurseAccounting.Mobile.Infrastructure.Accounting;

namespace PurseAccounting.Mobile.Application.Accounting;

internal class AccountingService : IAccountingService
{
    private static readonly TimeSpan _requestTimeout = TimeSpan.FromSeconds(2);
    private readonly IAccountClient _accountClient;

    public AccountingService(IAccountClient accountClient)
    {
        _accountClient = accountClient;
    }

    public async Task<bool> LoadAccount()
    {
        using var timeoutTokenSource = new CancellationTokenSource(_requestTimeout);
        var account = await _accountClient.GetAccount(timeoutTokenSource.Token);

        return account is not null;
    }
}
