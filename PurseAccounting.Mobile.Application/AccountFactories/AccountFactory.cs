using PurseAccounting.Mobile.Application.Models;
using PurseAccounting.Mobile.Infrastructure.Accounting;

namespace PurseAccounting.Mobile.Application.AccountFactories;

internal class AccountFactory : IAccountFactory
{
    private readonly IDateTimeService _dateTimeService;

    public AccountFactory(IDateTimeService dateTimeService)
    {
        _dateTimeService = dateTimeService;
    }

    public Account GetAccount(AccountDto account)
    {
        return new(_dateTimeService)
        {
            DailyDistributedAmount = new() { DayAmount = account.DayAmount, RestAmount = account.RestAmount },
            PlannedDate = new() { Value = account.PlannedDate },
            TimeZone = account.TimeZone,
        };
    }

    public Account GetAccount(Account account, DailyDistributedAmount amount)
    {
        return account with
        {
            DailyDistributedAmount = amount,
        };
    }
}
