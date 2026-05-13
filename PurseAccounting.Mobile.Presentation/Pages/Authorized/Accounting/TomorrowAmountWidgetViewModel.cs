using PurseAccounting.Mobile.Application.Calculators.TomorrowAmountCalculators;
using PurseAccounting.Mobile.Application.Context;
using PurseAccountinng.Mobile.Presentation.Services.Utils;
using ReactiveUI;

namespace PurseAccountinng.Mobile.Presentation.Pages.Authorized.Accounting;

internal class TomorrowAmountWidgetViewModel : ReactiveObject
{
    private readonly ITomorrowAmountCalculator _tomorrowAmountCalculator;

    private string _tomorrowAmountText = string.Empty;
    private bool _isTomorrowAmountVisible = true; // TODO: change to user settings when settings page will be created

    public string TomorrowAmountText
    {
        get => _tomorrowAmountText;
        set => this.RaiseAndSetIfChanged(ref _tomorrowAmountText, value, nameof(TomorrowAmountText));
    }

    public bool IsTomorrowAmountVisible
    {
        get => _isTomorrowAmountVisible;
        set => this.RaiseAndSetIfChanged(ref _isTomorrowAmountVisible, value, nameof(IsTomorrowAmountVisible));
    }

    public TomorrowAmountWidgetViewModel(ITomorrowAmountCalculator tomorrowAmountCalculator, IApplicationContext applicationContext)
    {
        _tomorrowAmountCalculator = tomorrowAmountCalculator;

        applicationContext.AccountChanged += OnAccountChanged;
        OnAccountChanged(null, applicationContext.Account);
    }

    private void OnAccountChanged(PurseAccounting.Mobile.Application.Models.Account? oldValue, PurseAccounting.Mobile.Application.Models.Account? newValue)
    {
        if (newValue is null)
            return;

        if (newValue.DaysCount == 0)
        {
            TomorrowAmountText = "Конец периода";
        }
        else
        {
            var tomorrowAmount = _tomorrowAmountCalculator.Calculate(newValue, TomorrowAmountCalculationStrategy.BetweenDays); // TODO: change to user settings when settings page will be created
            TomorrowAmountText = $"Сумма на завтра {AmountFormatter.FormatAmount(tomorrowAmount)} ₽";
        }
    }
}
