using PurseAccounting.Mobile.Application.AccountFactories;
using PurseAccounting.Mobile.Application.Context;
using PurseAccounting.Mobile.Infrastructure.Accounts;

namespace PurseAccounting.Mobile.Application.Accounts;

internal class AccountService : IAccountService
{
    private readonly IAccountClient _accountClient;
    private readonly IAccountFactory _accountFactory;
    private readonly IApplicationContext _applicationContext;

    public AccountService(IAccountClient accountClient, IAccountFactory accountFactory, IApplicationContext applicationContext)
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

    public async Task<UpdateAccountResult> UpdateAccount(long totalAmount, DateTime plannedDate, short timeZone, CancellationToken cancellationToken)
    {
        var response = await _accountClient.UpdateAccount(new()
        {
            FullAmount = totalAmount,
            PlannedDate = DateTime.SpecifyKind(plannedDate, DateTimeKind.Utc),
            TimeZone = timeZone,
        }, cancellationToken);

        return response.Match(result =>
        {
            if (_applicationContext.Account is not null)
                _applicationContext.Account = _accountFactory.CreateAccount(_applicationContext.Account, new() { DayAmount = result.DayAmount, RestAmount = result.RestAmount, ReservedAmount = result.ReservedAmount });

            return UpdateAccountResult.Success;
        },
        exception =>
        {
            return UpdateAccountResult.Failure;
        });
    }
}
