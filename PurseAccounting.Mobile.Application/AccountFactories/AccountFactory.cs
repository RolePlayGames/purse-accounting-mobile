using PurseAccounting.Mobile.Application.Context;

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
        return new()
        {
            DayAmount = account.DayAmount,
            AvaliableAmount = account.DayAmount + account.RestAmount,
            DaysCount = Math.Max((account.PlannedDate.Date - _dateTimeService.UtcNow.Date).Days + 1, 1),
        };
    }
}
