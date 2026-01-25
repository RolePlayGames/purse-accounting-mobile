using PurseAccounting.Mobile.Application.Accounting;
using PurseAccounting.Mobile.Application.Context;
using ReactiveUI;

namespace PurseAccountinng.Mobile.Presentation.Pages.Authorized.Accounting;

public class AccountWidgetViewModel : ReactiveObject
{
    private long _dayAmount = 0;
    private long _avaliableAmount = 0;
    private int _daysCount = 0;

    public long DayAmount
    {
        get => _dayAmount;
        set => this.RaiseAndSetIfChanged(ref _dayAmount, value, nameof(DayAmount));
    }

    public long AvaliableAmount
    {
        get => _avaliableAmount;
        set => this.RaiseAndSetIfChanged(ref _avaliableAmount, value, nameof(AvaliableAmount));
    }

    public int DaysCount
    {
        get => _daysCount;
        set => this.RaiseAndSetIfChanged(ref _daysCount, value, nameof(DaysCount));
    }

    public AccountWidgetViewModel(IAccountingService accountingService, IApplicationContext applicationContext)
    {
        applicationContext.AccountChanged += OnAccountChanged;
        OnAccountChanged(null, applicationContext.Account);

        Task.Run(() => accountingService.LoadAccount(CancellationToken.None));
    }

    private void OnAccountChanged(Account? oldValue, Account? newValue)
    {
        if (newValue is null)
            return;

        DayAmount = newValue.DayAmount;
        AvaliableAmount = newValue.AvaliableAmount;
        DaysCount = newValue.DaysCount;
    }
}
