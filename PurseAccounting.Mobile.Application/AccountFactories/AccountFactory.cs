using PurseAccounting.Mobile.Application.Models;
using PurseAccounting.Mobile.Infrastructure.Accounts;

namespace PurseAccounting.Mobile.Application.AccountFactories;

internal class AccountFactory : IAccountFactory
{
    private readonly IDateTimeService _dateTimeService;

    public AccountFactory(IDateTimeService dateTimeService)
    {
        _dateTimeService = dateTimeService;
    }

    public Account CreateAccount(AccountDto account)
    {
        return new(_dateTimeService)
        {
            DailyDistributedAmount = new() { DayAmount = account.DayAmount, RestAmount = account.RestAmount, ReservedAmount = 0 },
            PlannedDate = new() { Value = account.PlannedDate },
            TimeZone = account.TimeZone,
        };
    }

    public Account CreateAccount(Account account, DailyDistributedAmount amount)
    {
        return account with
        {
            DailyDistributedAmount = amount,
        };
    }
}
