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

    public AccountWidgetViewModel(IApplicationContext applicationContext)
    {
        applicationContext.AccountChanged += OnAccountChanged;
        OnAccountChanged(null, applicationContext.Account);
    }

    private void OnAccountChanged(PurseAccounting.Mobile.Application.Models.Account? oldValue, PurseAccounting.Mobile.Application.Models.Account? newValue)
    {
        if (newValue is null)
            return;

        DayAmount = newValue.DayAmount;
        AvaliableAmount = newValue.AvaliableAmount;
        DaysCount = newValue.DaysCount;
    }
}
