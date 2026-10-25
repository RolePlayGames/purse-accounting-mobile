using PurseAccounting.Mobile.Infrastructure.PlannedTransactions.Settings.Periods;
using PurseAccounting.Mobile.Infrastructure.TransactionCategories;
using PurseAccounting.Mobile.Infrastructure.Transactions;
using PurseAccountinng.Mobile.Presentation.Services;
using System.Globalization;
using System.Windows.Input;

namespace PurseAccountinng.Mobile.Presentation.Components.Pages.Authorized.PlannedTransactionSettings;

public partial class PlannedTransactionAttributesWidget : ContentView
{
    public static readonly BindableProperty TransactionNameProperty =
        BindableProperty.Create(
            nameof(TransactionName),
            typeof(string),
            typeof(PlannedTransactionAttributesWidget),
            string.Empty,
            BindingMode.TwoWay);

    public static readonly BindableProperty AmountProperty =
        BindableProperty.Create(
            nameof(Amount),
            typeof(int?),
            typeof(PlannedTransactionAttributesWidget),
            null,
            BindingMode.TwoWay,
            propertyChanged: OnAmountChanged);

    public static readonly BindableProperty TransactionChangeTypeProperty =
        BindableProperty.Create(
            nameof(TransactionChangeType),
            typeof(TransactionChangeType),
            typeof(PlannedTransactionAttributesWidget),
            TransactionChangeType.Withdrawal,
            BindingMode.TwoWay);

    public static readonly BindableProperty CategoriesProperty =
        BindableProperty.Create(
            nameof(Categories),
            typeof(IEnumerable<TransactionCategoryDto>),
            typeof(PlannedTransactionAttributesWidget),
            null,
            propertyChanged: OnCategoriesChanged);

    public static readonly BindableProperty SelectedCategoryIdProperty =
        BindableProperty.Create(
            nameof(SelectedCategoryId),
            typeof(long?),
            typeof(PlannedTransactionAttributesWidget),
            null,
            BindingMode.TwoWay);

    public static readonly BindableProperty SaveCommandProperty =
        BindableProperty.Create(
            nameof(SaveCommand),
            typeof(ICommand),
            typeof(PlannedTransactionAttributesWidget),
            null);

    public static readonly BindableProperty IsSaveEnabledProperty =
        BindableProperty.Create(
            nameof(IsSaveEnabled),
            typeof(bool),
            typeof(PlannedTransactionAttributesWidget),
            true);

    public static readonly BindableProperty PeriodProperty =
        BindableProperty.Create(
            nameof(Period),
            typeof(PeriodInfo),
            typeof(PlannedTransactionAttributesWidget),
            null,
            BindingMode.TwoWay,
            propertyChanged: OnPeriodChanged);

    public string TransactionName
    {
        get => (string)GetValue(TransactionNameProperty);
        set => SetValue(TransactionNameProperty, value);
    }

    public int? Amount
    {
        get => (int?)GetValue(AmountProperty);
        set => SetValue(AmountProperty, value);
    }

    public TransactionChangeType TransactionChangeType
    {
        get => (TransactionChangeType)GetValue(TransactionChangeTypeProperty);
        set => SetValue(TransactionChangeTypeProperty, value);
    }

    public IEnumerable<TransactionCategoryDto>? Categories
    {
        get => (IEnumerable<TransactionCategoryDto>?)GetValue(CategoriesProperty);
        set => SetValue(CategoriesProperty, value);
    }

    public long? SelectedCategoryId
    {
        get => (long?)GetValue(SelectedCategoryIdProperty);
        set => SetValue(SelectedCategoryIdProperty, value);
    }

    public ICommand? SaveCommand
    {
        get => (ICommand?)GetValue(SaveCommandProperty);
        set => SetValue(SaveCommandProperty, value);
    }

    public bool IsSaveEnabled
    {
        get => (bool)GetValue(IsSaveEnabledProperty);
        set => SetValue(IsSaveEnabledProperty, value);
    }

    public PeriodInfo? Period
    {
        get => (PeriodInfo?)GetValue(PeriodProperty);
        set => SetValue(PeriodProperty, value);
    }

    /// <summary>
    /// Поддерживаемые типы периодов в порядке их отображения в списке.
    /// Значения инициализируются лениво, чтобы не захватывать DateTime при создании статического поля.
    /// </summary>
    private static readonly Lazy<PeriodInfo[]> _availablePeriods = new(() =>
    [
        new OncePeriodInfo { Date = DateTime.Today },
        new DailyPeriodInfo(),
        new WeeklyPeriodInfo { DaysOfWeek = [DayOfWeek.Monday] },
        new MonthlyPeriodInfo { Day = DateTime.Today.Day },
        new AnnuallyPeriodInfo { Month = DateTime.Today.Month, Day = DateTime.Today.Day },
    ]);

    private static readonly string[] _periodLabels = ["Никогда", "Ежедневно", "Еженедельно", "Ежемесячно", "Ежегодно"];

    private bool _isUpdatingAmount = false;
    private bool _isUpdatingPeriodSelection = false;

    public PlannedTransactionAttributesWidget()
    {
        InitializeComponent();

        SetupAmountBinding();
        InitializePeriodPicker();

        UpdateAmountDisplay(Amount);
        UpdatePeriodDisplay(Period);
    }

    private static void OnPeriodChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is PlannedTransactionAttributesWidget widget)
        {
            widget.UpdatePeriodDisplay(newValue as PeriodInfo);
        }
    }

    private static void OnAmountChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is PlannedTransactionAttributesWidget widget)
        {
            widget.UpdateAmountDisplay(newValue as int?);
        }
    }

    private static void OnCategoriesChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is PlannedTransactionAttributesWidget widget && newValue is IEnumerable<TransactionCategoryDto> categories)
        {
            widget.CategorySelector.ItemsSource = categories.ToList();
        }
    }

    private void SetupAmountBinding()
    {
        AmountTextField.PropertyChanged += (sender, e) =>
        {
            if (e.PropertyName == nameof(AmountTextField.Text) && !_isUpdatingAmount)
            {
                ParseAndSetAmount(AmountTextField.Text);
            }
        };
    }

    private void UpdateAmountDisplay(int? amountInCents)
    {
        _isUpdatingAmount = true;

        var text = amountInCents.HasValue
            ? (amountInCents.Value / 100m).ToString("0.##", CultureInfo.CreateSpecificCulture("ru-RU"))
            : null;

        if (AmountTextField.Text != text)
            AmountTextField.Text = text;

        _isUpdatingAmount = false;
    }

    private void ParseAndSetAmount(string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            Amount = null;
        }
        else if (AmountParser.TryParseToCents(AmountParser.FilterInput(text), out var value) && Amount != value)
        {
            Amount = value;
        }
    }

    private void InitializePeriodPicker()
    {
        foreach (var label in _periodLabels)
        {
            PeriodPicker.Items.Add(label);
        }
    }

    private void OnPeriodSelectedIndexChanged(object sender, EventArgs e)
    {
        if (_isUpdatingPeriodSelection)
            return;

        var periods = _availablePeriods.Value;

        if (PeriodPicker.SelectedIndex >= 0 && PeriodPicker.SelectedIndex < periods.Length)
        {
            Period = CreatePeriodInstance(periods[PeriodPicker.SelectedIndex]);
        }
    }

    /// <summary>
    /// Создает новый экземпляр периода выбранного типа с допустимыми значениями по умолчанию.
    /// </summary>
    private static PeriodInfo CreatePeriodInstance(PeriodInfo template)
    {
        var today = DateTime.Today;

        return template switch
        {
            OncePeriodInfo => new OncePeriodInfo { Date = today },
            DailyPeriodInfo => new DailyPeriodInfo(),
            WeeklyPeriodInfo => new WeeklyPeriodInfo { DaysOfWeek = [DayOfWeek.Monday] },
            MonthlyPeriodInfo => new MonthlyPeriodInfo { Day = today.Day },
            AnnuallyPeriodInfo => new AnnuallyPeriodInfo { Month = today.Month, Day = today.Day },
            _ => throw new ArgumentOutOfRangeException(nameof(template)),
        };
    }

    private void UpdatePeriodDisplay(PeriodInfo? period)
    {
        if (period is null)
            return;

        var index = Array.FindIndex(_availablePeriods.Value, p => p.GetType() == period.GetType());

        PeriodLabel.Text = index >= 0 ? _periodLabels[index] : string.Empty;

        if (index >= 0 && PeriodPicker.SelectedIndex != index)
        {
            _isUpdatingPeriodSelection = true;
            PeriodPicker.SelectedIndex = index;
            _isUpdatingPeriodSelection = false;
        }
    }
}
