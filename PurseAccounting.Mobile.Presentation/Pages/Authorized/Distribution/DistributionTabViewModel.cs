using PurseAccounting.Mobile.Application.Context;
using PurseAccounting.Mobile.Application.Distribution;
using PurseAccountinng.Mobile.Presentation.Pages.Authorized.Accounting;
using PurseAccountinng.Mobile.Presentation.Services.Notifications;
using PurseAccountinng.Mobile.Presentation.Services.Tabs;
using PurseAccountinng.Mobile.Presentation.Services.Utils;
using ReactiveUI;
using System.Windows.Input;

namespace PurseAccountinng.Mobile.Presentation.Pages.Authorized.Distribution;

public class DistributionTabViewModel : ReactiveObject
{
    private readonly INotificationService _notificationService;
    private readonly IDistributionService _distributionService;
    private readonly ITabNavigator _tabNavigator;

    private PurseAccounting.Mobile.Application.Models.Account? _account;
    private AvailableUserChoiceDistributionStrategyInfo? _availableUserChoiceDistributionStrategy;

    private string? _allToTodayDistributedDayAmount;
    private string? _betweenDaysDistributedDayAmount;
    private string? _restDayAmount;

    public string AllToTodayDistributedDayAmount
    {
        get => _allToTodayDistributedDayAmount ?? string.Empty;
        set => this.RaiseAndSetIfChanged(ref _allToTodayDistributedDayAmount, value, nameof(AllToTodayDistributedDayAmount));
    }

    public string BetweenDaysDistributedDayAmount
    {
        get => _betweenDaysDistributedDayAmount ?? string.Empty;
        set => this.RaiseAndSetIfChanged(ref _betweenDaysDistributedDayAmount, value, nameof(BetweenDaysDistributedDayAmount));
    }

    public string RestDayAmount
    {
        get => _restDayAmount ?? string.Empty;
        set => this.RaiseAndSetIfChanged(ref _restDayAmount, value, nameof(RestDayAmount));
    }

    public ICommand DistributeAllToTodayCommand { get; }

    public ICommand DistributeBetweenDaysCommand { get; }

    public DistributionTabViewModel(
        INotificationService notificationService,
        IDistributionService distributionService,
        ITabNavigator tabNavigator,
        IApplicationContext applicationContext)
    {
        _notificationService = notificationService;
        _distributionService = distributionService;
        _tabNavigator = tabNavigator;

        DistributeAllToTodayCommand = new Command(OnDistributeAllToToday);
        DistributeBetweenDaysCommand = new Command(OnDistributeBetweenDays);

        if (applicationContext.AvailableUserChoiceDistributionStrategy is not null)
            OnAvailableUserChoiceDistributionStrategyChanged(null, applicationContext.AvailableUserChoiceDistributionStrategy);

        if (applicationContext.Account is not null)
            OnAccountChanged(null, applicationContext.Account);

        applicationContext.AccountChanged += OnAccountChanged;
        applicationContext.AvailableUserChoiceDistributionStrategyChanged += OnAvailableUserChoiceDistributionStrategyChanged;
    }

    private static long CalculateRestDayAmount(PurseAccounting.Mobile.Application.Models.Account account, AvailableUserChoiceDistributionStrategyInfo info)
    {
        if (account.DaysCount > 1)
        {
            var totalAmount = info.BetweenDaysDistributedDayAmount * account.DaysCount;
            var restAmount = totalAmount - info.AllToTodayDistributedDayAmount;
            var oldDayAmount = restAmount / (account.DaysCount - 1);

            return info.AllToTodayDistributedDayAmount - oldDayAmount;
        }

        return 0;
    }

    private void OnAccountChanged(PurseAccounting.Mobile.Application.Models.Account? oldValue, PurseAccounting.Mobile.Application.Models.Account? newValue)
    {
        if (newValue is null)
            return;

        _account = newValue;

        if (_availableUserChoiceDistributionStrategy is null)
            return;

        var restDayAmount = CalculateRestDayAmount(newValue, _availableUserChoiceDistributionStrategy);

        RestDayAmount = AmountFormatter.FormatAmount(restDayAmount);
    }

    private void OnAvailableUserChoiceDistributionStrategyChanged(AvailableUserChoiceDistributionStrategyInfo? oldValue, AvailableUserChoiceDistributionStrategyInfo? newValue)
    {
        if (newValue is null)
            return;

        _availableUserChoiceDistributionStrategy = newValue;

        AllToTodayDistributedDayAmount = AmountFormatter.FormatAmount(newValue.AllToTodayDistributedDayAmount);
        BetweenDaysDistributedDayAmount = AmountFormatter.FormatAmount(newValue.BetweenDaysDistributedDayAmount);

        OnAccountChanged(null, _account);
    }

    private async void OnDistributeAllToToday()
    {
        var result = await _distributionService.DistributeAllToToday(CancellationToken.None);

        if (result == DistributionResult.Success)
        {
            _notificationService.ShowSuccess("Распределение выполнено успешно");
            _tabNavigator.ChangeTabTo<AccountingTab>();
        }
        else if (result == DistributionResult.Failed)
        {
            _notificationService.ShowError("Распределение не удалось. Повторите попытку позже");
        }
    }

    private async void OnDistributeBetweenDays()
    {
        var result = await _distributionService.DistributeBetweenDays(CancellationToken.None);

        if (result == DistributionResult.Success)
        {
            _notificationService.ShowSuccess("Распределение выполнено успешно");
            _tabNavigator.ChangeTabTo<AccountingTab>();
        }
        else if (result == DistributionResult.Failed)
        {
            _notificationService.ShowError("Распределение не удалось. Повторите попытку позже");
        }
    }
}
