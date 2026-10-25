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

    public static readonly BindableProperty PlannedDateProperty =
        BindableProperty.Create(
            nameof(PlannedDate),
            typeof(DateTime?),
            typeof(PlannedTransactionAttributesWidget),
            null,
            BindingMode.TwoWay,
            propertyChanged: OnPlannedDateChanged);

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
    /// Планируемая дата транзакции (используется при периоде OncePeriodInfo).
    /// </summary>
    public DateTime? PlannedDate
    {
        get => (DateTime?)GetValue(PlannedDateProperty);
        set => SetValue(PlannedDateProperty, value);
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

    /// <summary>
    /// Дни недели в порядке отображения (с понедельника) и их полные названия на русском языке.
    /// </summary>
    private static readonly (DayOfWeek Day, string FullName, string ShortName)[] _weekDays =
    [
        (DayOfWeek.Monday, "Понедельник", "Пн"),
        (DayOfWeek.Tuesday, "Вторник", "Вт"),
        (DayOfWeek.Wednesday, "Среда", "Ср"),
        (DayOfWeek.Thursday, "Четверг", "Чт"),
        (DayOfWeek.Friday, "Пятница", "Пт"),
        (DayOfWeek.Saturday, "Суббота", "Сб"),
        (DayOfWeek.Sunday, "Воскресенье", "Вс"),
    ];

    private static readonly string[] _monthNames =
    [
        "Январь",
        "Февраль",
        "Март",
        "Апрель",
        "Май",
        "Июнь",
        "Июль",
        "Август",
        "Сентябрь",
        "Октябрь",
        "Ноябрь",
        "Декабрь",
    ];

    private readonly HashSet<DayOfWeek> _selectedWeekDays = [DayOfWeek.Monday];

    private bool _isUpdatingAmount = false;
    private bool _isUpdatingPeriodSelection = false;
    private bool _isUpdatingMonthDaySelection = false;
    private bool _isUpdatingYearMonthSelection = false;
    private bool _isUpdatingOnceDate = false;

    public PlannedTransactionAttributesWidget()
    {
        InitializeComponent();

        SetupAmountBinding();
        InitializePeriodPicker();
        InitializeMonthDayPicker();
        InitializeYearMonthPicker();
        SetupWeeklyDaysPanel();

        UpdateAmountDisplay(Amount);
        UpdatePeriodDisplay(Period);

        if (PlannedDate is DateTime plannedDate)
        {
            UpdateOnceDateDisplay(plannedDate);
        }
    }

    private static void OnPeriodChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is PlannedTransactionAttributesWidget widget)
        {
            widget.UpdatePeriodDisplay(newValue as PeriodInfo);
        }
    }

    private static void OnPlannedDateChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is PlannedTransactionAttributesWidget widget && newValue is DateTime date)
        {
            widget.UpdateOnceDateDisplay(date);
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

    #region Выбор типа периода

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
    /// Для еженедельного периода сохраняются уже выбранные дни недели.
    /// </summary>
    private PeriodInfo CreatePeriodInstance(PeriodInfo template)
    {
        var today = DateTime.Today;

        return template switch
        {
            OncePeriodInfo => new OncePeriodInfo { Date = today },
            DailyPeriodInfo => new DailyPeriodInfo(),
            WeeklyPeriodInfo => new WeeklyPeriodInfo { DaysOfWeek = GetSortedSelectedWeekDays() },
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

        UpdatePeriodSectionsVisibility(period);
    }

    /// <summary>
    /// Показывает контролы, зависимые от выбранного типа периода.
    /// </summary>
    private void UpdatePeriodSectionsVisibility(PeriodInfo period)
    {
        OnceDateSection.IsVisible = period is OncePeriodInfo;
        WeeklyDaysSection.IsVisible = period is WeeklyPeriodInfo;
        // Число нужно и для ежемесячного, и для ежегодного периодов
        MonthDaySection.IsVisible = period is MonthlyPeriodInfo or AnnuallyPeriodInfo;
        YearMonthSection.IsVisible = period is AnnuallyPeriodInfo;

        switch (period)
        {
            case OncePeriodInfo once:
                UpdateOnceDateDisplay(once.Date);
                break;
            case WeeklyPeriodInfo weekly:
                SyncSelectedWeekDays(weekly.DaysOfWeek);
                break;
            case MonthlyPeriodInfo monthly:
                UpdateMonthDayDisplay(monthly.Day);
                break;
            case AnnuallyPeriodInfo annually:
                UpdateMonthDayDisplay(annually.Day);
                UpdateYearMonthDisplay(annually.Month);
                break;
        }
    }

    #endregion

    #region Дата (OncePeriodInfo)

    private void OnOnceDateSelected(object sender, DateChangedEventArgs e)
    {
        if (_isUpdatingOnceDate)
            return;

        PlannedDate = e.NewDate;

        if (Period is OncePeriodInfo once)
        {
            Period = once with { Date = e.NewDate };
        }
    }

    private void UpdateOnceDateDisplay(DateTime date)
    {
        DateLabel.Text = date.ToString("dd.MM.yyyy");

        _isUpdatingOnceDate = true;

        if (NativeDatePicker.Date != date)
            NativeDatePicker.Date = date;

        _isUpdatingOnceDate = false;
    }

    #endregion

    #region Дни недели (WeeklyPeriodInfo)

    /// <summary>
    /// Имитирует выпадающий список с мультиселектом: по клику на поле раскрывается
    /// нативный Picker с полным названием дня недели, каждый выбор добавляет день в набор.
    /// </summary>
    private void SetupWeeklyDaysPanel()
    {
        var picker = new Picker
        {
            BackgroundColor = Colors.Transparent,
            TextColor = Colors.Transparent,
            Opacity = 0.01,
            VerticalOptions = LayoutOptions.Fill,
            HorizontalOptions = LayoutOptions.Fill,
        };

        foreach (var (_, fullName, _) in _weekDays)
        {
            picker.Items.Add(fullName);
        }

        picker.SelectedIndexChanged += (sender, e) =>
        {
            if (picker.SelectedIndex < 0 || picker.SelectedIndex >= _weekDays.Length)
                return;

            _selectedWeekDays.Add(_weekDays[picker.SelectedIndex].Day);
            ApplySelectedWeekDays();

            // Сбрасываем выбор, чтобы повторный клик по тому же дню снова вызвал событие
            picker.SelectedIndex = -1;
        };

        // Picker перекрывает всё поле — нажатие открывает список дней
        Grid.SetColumnSpan(picker, 2);
        WeeklyDaysSection.Children[0].Children[0].Add(picker);

        UpdateWeeklyDaysLabel();
    }

    private void SyncSelectedWeekDays(DayOfWeek[] days)
    {
        _selectedWeekDays.Clear();

        foreach (var day in days)
        {
            _selectedWeekDays.Add(day);
        }

        if (_selectedWeekDays.Count == 0)
            _selectedWeekDays.Add(DayOfWeek.Monday);

        UpdateWeeklyDaysLabel();
    }

    private DayOfWeek[] GetSortedSelectedWeekDays()
    {
        return _selectedWeekDays
            .OrderBy(d => d == DayOfWeek.Sunday ? 6 : (int)d - 1)
            .ToArray();
    }

    private void ApplySelectedWeekDays()
    {
        UpdateWeeklyDaysLabel();

        if (Period is WeeklyPeriodInfo weekly)
        {
            var days = GetSortedSelectedWeekDays();

            if (!days.SequenceEqual(weekly.DaysOfWeek))
            {
                Period = weekly with { DaysOfWeek = days };
            }
        }
    }

    private void UpdateWeeklyDaysLabel()
    {
        var sortedDays = GetSortedSelectedWeekDays();
        var shortNames = sortedDays.Select(d => _weekDays.First(x => x.Day == d).ShortName);

        WeeklyDaysLabel.Text = string.Join(", ", shortNames);
    }

    #endregion

    #region Число месяца (MonthlyPeriodInfo, AnnuallyPeriodInfo)

    private void InitializeMonthDayPicker()
    {
        for (var day = 1; day <= 31; day++)
        {
            MonthDayPicker.Items.Add($"{day} число");
        }
    }

    private void OnMonthDaySelectedIndexChanged(object sender, EventArgs e)
    {
        if (_isUpdatingMonthDaySelection)
            return;

        var day = MonthDayPicker.SelectedIndex + 1;

        if (day < 1 || day > 31)
            return;

        switch (Period)
        {
            case MonthlyPeriodInfo monthly:
                Period = monthly with { Day = day };
                break;
            case AnnuallyPeriodInfo annually:
                Period = annually with { Day = day };
                break;
        }
    }

    private void UpdateMonthDayDisplay(int day)
    {
        MonthDayLabel.Text = $"{day} число";

        var index = day - 1;

        if (index >= 0 && MonthDayPicker.SelectedIndex != index)
        {
            _isUpdatingMonthDaySelection = true;
            MonthDayPicker.SelectedIndex = index;
            _isUpdatingMonthDaySelection = false;
        }
    }

    #endregion

    #region Месяц (AnnuallyPeriodInfo)

    private void InitializeYearMonthPicker()
    {
        foreach (var name in _monthNames)
        {
            YearMonthPicker.Items.Add(name);
        }
    }

    private void OnYearMonthSelectedIndexChanged(object sender, EventArgs e)
    {
        if (_isUpdatingYearMonthSelection)
            return;

        var month = YearMonthPicker.SelectedIndex + 1;

        if (month < 1 || month > 12)
            return;

        if (Period is AnnuallyPeriodInfo annually)
        {
            Period = annually with { Month = month };
        }
    }

    private void UpdateYearMonthDisplay(int month)
    {
        var index = month - 1;

        YearMonthLabel.Text = index >= 0 && index < _monthNames.Length ? _monthNames[index] : string.Empty;

        if (index >= 0 && YearMonthPicker.SelectedIndex != index)
        {
            _isUpdatingYearMonthSelection = true;
            YearMonthPicker.SelectedIndex = index;
            _isUpdatingYearMonthSelection = false;
        }
    }

    #endregion
}
