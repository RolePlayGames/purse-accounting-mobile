using PurseAccounting.Mobile.Application.Accounts;
using PurseAccounting.Mobile.Application.Calculators.AmountsCalculators;
using PurseAccounting.Mobile.Application.Calculators.DaysCountCalculators;
using PurseAccounting.Mobile.Application.Context;
using PurseAccountinng.Mobile.Presentation.Services.Notifications;
using ReactiveUI;
using System.Windows.Input;

namespace PurseAccountinng.Mobile.Presentation.Pages.Authorized.Account;

public class AccountAttributesWidgetViewModel : ReactiveObject
{
    private readonly INotificationService _notificationService;
    private readonly IAmountsCalculator _amountsCalculator;
    private readonly IAccountService _accountService;

    private long? _totalAmount = null;
    private DateTime _plannedDate = DateTime.Now;
    private short _timeZoneOffset = 0;
    private bool _isSaveEnabled = false;
    private long _dayAmount = 0;
    private long _avaliableAmount = 0;
    private int _daysCount = 0;

    public long? TotalAmount
    {
        get => _totalAmount;
        set => this.RaiseAndSetIfChanged(ref _totalAmount, value, nameof(TotalAmount));
    }

    public DateTime PlannedDate
    {
        get => _plannedDate;
        set => this.RaiseAndSetIfChanged(ref _plannedDate, value, nameof(PlannedDate));
    }

    public short TimeZoneOffset
    {
        get => _timeZoneOffset;
        set => this.RaiseAndSetIfChanged(ref _timeZoneOffset, value, nameof(TimeZoneOffset));
    }

    public bool IsSaveEnabled
    {
        get => _isSaveEnabled;
        set => this.RaiseAndSetIfChanged(ref _isSaveEnabled, value, nameof(IsSaveEnabled));
    }

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

    public ICommand SaveCommand { get; }

    public AccountAttributesWidgetViewModel(
        INotificationService notificationService,
        IApplicationContext applicationContext,
        IDaysCountCalculators daysCountCalculators,
        IDateTimeService dateTimeService,
        IAmountsCalculator amountsCalculator,
        IAccountService accountService)
    {
        _notificationService = notificationService;
        _amountsCalculator = amountsCalculator;
        _accountService = accountService;

        SaveCommand = new Command(OnSave, () => IsSaveEnabled);

        this.WhenAnyValue(x => x.TotalAmount, x => x.PlannedDate)
            .Subscribe(_ => ValidateForm());

        this.WhenAnyValue(x => x.TotalAmount)
            .Subscribe(x => AvaliableAmount = x ?? 0); // temporary equals

        this.WhenAnyValue(x => x.PlannedDate, x => x.TimeZoneOffset)
            .Subscribe(x => DaysCount = daysCountCalculators.Calculate(dateTimeService.UtcNow.AddHours(TimeZoneOffset), PlannedDate.AddHours(TimeZoneOffset)));

        this.WhenAnyValue(x => x.AvaliableAmount, x => x.DaysCount)
            .Subscribe(_ => CalculateDayAmount());

        applicationContext.AccountChanged += OnAccountChanged;
        OnAccountChanged(null, applicationContext.Account);
    }

    private void ValidateForm()
    {
        var isDateValid = DaysCount > 0;
        var isAmountValid = TotalAmount > 0;

        IsSaveEnabled = isDateValid && isAmountValid;
    }

    private async void OnSave()
    {
        if (TotalAmount is null)
            return;

        var result = await _accountService.UpdateAccount(TotalAmount.Value, PlannedDate, TimeZoneOffset, CancellationToken.None);

        if (result == UpdateAccountResult.Success)
        {
            _notificationService.ShowSuccess("Счет сохранен");
        }
        else
        {
            _notificationService.ShowError("Сохранение не удалось. Повторите попытку позже");
        }
    }

    private void OnAccountChanged(PurseAccounting.Mobile.Application.Models.Account? oldValue, PurseAccounting.Mobile.Application.Models.Account? newValue)
    {
        if (newValue is null)
            return;

        TotalAmount = null;
        DayAmount = newValue.DayAmount;
        AvaliableAmount = newValue.AvaliableAmount;
        DaysCount = newValue.DaysCount;
        TimeZoneOffset = newValue.TimeZone;
        PlannedDate = newValue.PlannedDate.Value;
    }

    private void CalculateDayAmount()
    {
        if (AvaliableAmount >= 0 && DaysCount >= 0)
            DayAmount = _amountsCalculator.CalculateAmounts(AvaliableAmount, DaysCount).DayAmount;
    }
}
