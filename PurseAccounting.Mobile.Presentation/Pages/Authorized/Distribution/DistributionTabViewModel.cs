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

    private string? _allToTodayDistributedDayAmount;
    private string? _betweenDaysDistributedDayAmount;
    private string? _restDayAmount;

    public event EventHandler? DistributionSucceeded;

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
        IApplicationContext applicationContext,
        AvailableUserChoiceDistributionStrategyInfo availableUserChoiceDistributionStrategy)
    {
        _notificationService = notificationService;
        _distributionService = distributionService;

        DistributeAllToTodayCommand = new Command(OnDistributeAllToToday);
        DistributeBetweenDaysCommand = new Command(OnDistributeBetweenDays);

        AllToTodayDistributedDayAmount = AmountFormatter.FormatAmount(availableUserChoiceDistributionStrategy.AllToTodayDistributedDayAmount);
        BetweenDaysDistributedDayAmount = AmountFormatter.FormatAmount(availableUserChoiceDistributionStrategy.BetweenDaysDistributedDayAmount);

        if (applicationContext.Account is not null)
        {
            var restDayAmount = CalculateRestDayAmount(applicationContext.Account, availableUserChoiceDistributionStrategy);

            RestDayAmount = AmountFormatter.FormatAmount(restDayAmount);
        }
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

    private async void OnDistributeAllToToday()
    {
        var result = await _distributionService.DistributeAllToToday(CancellationToken.None);

        if (result == DistributionResult.Success)
        {
            _notificationService.ShowSuccess("Распределение выполнено успешно");
            DistributionSucceeded?.Invoke(this, EventArgs.Empty);
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
            DistributionSucceeded?.Invoke(this, EventArgs.Empty);
        }
        else if (result == DistributionResult.Failed)
        {
            _notificationService.ShowError("Распределение не удалось. Повторите попытку позже");
        }
    }
}
