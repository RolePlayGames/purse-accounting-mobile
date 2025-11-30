using PurseAccounting.Mobile.Application.Models;

namespace PurseAccounting.Mobile.Application.AccountFactories;

internal class AccountFactory : IAccountFactory
{
    private readonly IDateTimeService _dateTimeService;

    public AccountFactory(IDateTimeService dateTimeService)
    {
        _dateTimeService = dateTimeService;
    }

    public Account GetAccount(Infrastructure.Accounting.Account account)
    {
        return new(_dateTimeService)
        {
            DailyDistributedAmount = new() { DayAmount = account.DayAmount, RestAmount = account.RestAmount },
            PlannedDate = new() { Value = account.PlannedDate },
            TimeZone = account.TimeZone,
        };
    }
}
