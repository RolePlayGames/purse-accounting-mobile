using PurseAccounting.Mobile.Application.Context;
using PurseAccounting.Mobile.Application.Distribution;
using PurseAccountinng.Mobile.Presentation.Services.Notifications;
using PurseAccountinng.Mobile.Presentation.Services.Utils;
using ReactiveUI;
using System.Windows.Input;

namespace PurseAccountinng.Mobile.Presentation.Pages.Authorized.Distribution;

public class DistributionTabViewModel : ReactiveObject
{
    private readonly INotificationService _notificationService;
    private readonly IDistributionService _distributionService;
    private readonly IApplicationContext _applicationContext;

    private long _allToTodayDistributedDayAmount;
    private long _betweenDaysDistributedDayAmount;
    private string _restDayAmount;

    public long AllToTodayDistributedDayAmount
    {
        get => _allToTodayDistributedDayAmount;
        set => this.RaiseAndSetIfChanged(ref _allToTodayDistributedDayAmount, value, nameof(AllToTodayDistributedDayAmount));
    }

    public long BetweenDaysDistributedDayAmount
    {
        get => _betweenDaysDistributedDayAmount;
        set => this.RaiseAndSetIfChanged(ref _betweenDaysDistributedDayAmount, value, nameof(BetweenDaysDistributedDayAmount));
    }

    public string RestDayAmount
    {
        get => _restDayAmount;
        set => this.RaiseAndSetIfChanged(ref _restDayAmount, value, nameof(RestDayAmount));
    }

    public ICommand DistributeAllToTodayCommand { get; }

    public ICommand DistributeBetweenDaysCommand { get; }

    public DistributionTabViewModel(
        INotificationService notificationService,
        IDistributionService distributionService,
        IApplicationContext applicationContext,
        AvailableUserChoiceDistributionStrategyInfo? availableUserChoiceDistributionStrategy = null)
    {
        _notificationService = notificationService;
        _distributionService = distributionService;
        _applicationContext = applicationContext;

        DistributeAllToTodayCommand = new Command(OnDistributeAllToToday);
        DistributeBetweenDaysCommand = new Command(OnDistributeBetweenDays);

        _applicationContext.AccountChanged += OnAccountChanged;

        if (availableUserChoiceDistributionStrategy is not null)
        {
            AllToTodayDistributedDayAmount = availableUserChoiceDistributionStrategy.AllToTodayDistributedDayAmount;
            BetweenDaysDistributedDayAmount = availableUserChoiceDistributionStrategy.BetweenDaysDistributedDayAmount;
            RestDayAmount = AmountFormatter.FormatAmount(availableUserChoiceDistributionStrategy.BetweenDaysDistributedDayAmount);
        }
        else
        {
            OnAccountChanged(null, applicationContext.Account);
        }
    }

    private async void OnDistributeAllToToday()
    {
        var result = await _distributionService.DistributeAllToToday(CancellationToken.None);

        if (result == DistributionResult.Success)
            _notificationService.ShowSuccess("Распределение выполнено успешно");
        else if (result == DistributionResult.Failed)
            _notificationService.ShowError("Распределение не удалось. Повторите попытку позже");
    }

    private async void OnDistributeBetweenDays()
    {
        var result = await _distributionService.DistributeBetweenDays(CancellationToken.None);

        if (result == DistributionResult.Success)
            _notificationService.ShowSuccess("Распределение выполнено успешно");
        else if (result == DistributionResult.Failed)
            _notificationService.ShowError("Распределение не удалось. Повторите попытку позже");
    }

    private void OnAccountChanged(PurseAccounting.Mobile.Application.Models.Account? oldValue, PurseAccounting.Mobile.Application.Models.Account? newValue)
    {
        if (newValue is null)
            return;

        RestDayAmount = AmountFormatter.FormatAmount(newValue.DailyDistributedAmount.DayAmount);
    }
}
