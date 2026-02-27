using PurseAccountinng.Mobile.Presentation.Services;
using System.Globalization;
using System.Windows.Input;

namespace PurseAccountinng.Mobile.Presentation.Pages.Authorized.Account;

public partial class AccountAttributesWidget : ContentView
{
    public static readonly BindableProperty TotalAmountProperty =
        BindableProperty.Create(
            nameof(TotalAmount),
            typeof(long?),
            typeof(AccountAttributesWidget),
            null,
            BindingMode.TwoWay,
            propertyChanged: OnTotalAmountChanged);

    public static readonly BindableProperty PlannedDateProperty =
        BindableProperty.Create(
            nameof(PlannedDate),
            typeof(DateTime),
            typeof(AccountAttributesWidget),
            DateTime.Today,
            BindingMode.TwoWay,
            propertyChanged: OnPlannedDateChanged);

    public static readonly BindableProperty TimeZoneOffsetProperty =
        BindableProperty.Create(
            nameof(TimeZoneOffset),
            typeof(short),
            typeof(AccountAttributesWidget),
            (short)0,
            BindingMode.TwoWay,
            propertyChanged: OnTimeZoneOffsetChanged);

    public static readonly BindableProperty SaveCommandProperty =
        BindableProperty.Create(
            nameof(SaveCommand),
            typeof(System.Windows.Input.ICommand),
            typeof(AccountAttributesWidget),
            null);

    public static readonly BindableProperty IsSaveEnabledProperty =
        BindableProperty.Create(
            nameof(IsSaveEnabled),
            typeof(bool),
            typeof(AccountAttributesWidget),
            true);

    private const short _minTimeZoneIndex = -12;
    private const short _maxTimeZoneIndex = 14;

    private static readonly Dictionary<short, string> _timeZoneMap = new()
    {
        { -12, "(UTC-12:00) Международная линия перемены дат" },
        { -11, "(UTC-11:00) Самоа" },
        { -10, "(UTC-10:00) Гавайи" },
        { -9, "(UTC-09:00) Аляска" },
        { -8, "(UTC-08:00) Тихоокеанское время (США)" },
        { -7, "(UTC-07:00) Горное время (США)" },
        { -6, "(UTC-06:00) Центральное время (США)" },
        { -5, "(UTC-05:00) Восточное время (США)" },
        { -4, "(UTC-04:00) Атлантическое время" },
        { -3, "(UTC-03:00) Бразилия, Аргентина" },
        { -2, "(UTC-02:00) Среднеатлантическое время" },
        { -1, "(UTC-01:00) Азорские острова" },
        { 0, "(UTC+00:00) Лондон, Рейкьявик" },
        { 1, "(UTC+01:00) Берлин, Париж, Рим" },
        { 2, "(UTC+02:00) Киев, Хельсинки" },
        { 3, "(UTC+03:00) Москва, Санкт-Петербург" },
        { 4, "(UTC+04:00) Самара, Ереван" },
        { 5, "(UTC+05:00) Екатеринбург, Исламабад" },
        { 6, "(UTC+06:00) Омск, Дакка" },
        { 7, "(UTC+07:00) Новосибирск, Бангкок" },
        { 8, "(UTC+08:00) Красноярск, Пекин, Сингапур" },
        { 9, "(UTC+09:00) Иркутск, Токио, Сеул" },
        { 10, "(UTC+10:00) Владивосток, Сидней" },
        { 11, "(UTC+11:00) Магадан" },
        { 12, "(UTC+12:00) Камчатка, Окленд" },
        { 13, "(UTC+13:00) Самоа (летом)" },
        { 14, "(UTC+14:00) Острова Лайн" },
    };

    public long? TotalAmount
    {
        get => (long?)GetValue(TotalAmountProperty);
        set => SetValue(TotalAmountProperty, value);
    }

    public DateTime PlannedDate
    {
        get => (DateTime)GetValue(PlannedDateProperty);
        set => SetValue(PlannedDateProperty, value);
    }

    public short TimeZoneOffset
    {
        get => (short)GetValue(TimeZoneOffsetProperty);
        set => SetValue(TimeZoneOffsetProperty, value);
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

    private bool _isUpdatingAmount = false;

    public AccountAttributesWidget()
    {
        InitializeComponent();

        NativeDatePicker.MinimumDate = DateTime.Today.Date;
        NativeDatePicker.DateSelected += OnDateSelected;

        SetupAmountBinding();
        InitializeTimeZonePicker();

        UpdateDateLabel(PlannedDate);
        UpdateTimeZoneDisplay(TimeZoneOffset);
        UpdateAmountDisplay(TotalAmount);
    }

    private static void OnTotalAmountChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is AccountAttributesWidget widget && newValue is long amount)
        {
            widget.UpdateAmountDisplay(amount);
        }
    }

    private static void OnPlannedDateChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is AccountAttributesWidget widget && newValue is DateTime date)
        {
            widget.NativeDatePicker.Date = date.Date;
            widget.UpdateDateLabel(date);
        }
    }

    private static void OnTimeZoneOffsetChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is AccountAttributesWidget widget && newValue is short offset)
        {
            widget.UpdateTimeZoneDisplay(offset);
        }
    }

    private static string GetTimeZoneDisplayName(short offset)
    {
        return _timeZoneMap.TryGetValue(offset, out var name) ? name : $"(UTC{offset:+00;-00}:00) Часовой пояс";
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

    private void OnDateSelected(object? sender, DateChangedEventArgs e)
    {
        PlannedDate = e.NewDate.Date;
    }

    private void OnTimeZoneSelectedIndexChanged(object sender, EventArgs e)
    {
        if (TimeZonePicker.SelectedIndex >= 0)
        {
            TimeZoneOffset = Convert.ToInt16(TimeZonePicker.SelectedIndex + _minTimeZoneIndex);
        }
    }

    private void UpdateDateLabel(DateTime date)
    {
        DateLabel.Text = date.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
    }

    private void InitializeTimeZonePicker()
    {
        for (short offset = _minTimeZoneIndex; offset <= _maxTimeZoneIndex; offset++)
        {
            TimeZonePicker.Items.Add(GetTimeZoneDisplayName(offset));
        }
    }

    private void UpdateTimeZoneDisplay(short offset)
    {
        TimeZoneLabel.Text = GetTimeZoneDisplayName(offset);

        var index = offset - _minTimeZoneIndex; // convert offset to index
        if (index >= 0 && index < TimeZonePicker.Items.Count && TimeZonePicker.SelectedIndex != index)
        {
            TimeZonePicker.SelectedIndex = index;
        }
    }

    private void UpdateAmountDisplay(long? amountInCents)
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
            TotalAmount = null;
        }
        else if (AmountParser.TryParseToCents(AmountParser.FilterInput(text), out var value) && TotalAmount != value)
        {
            TotalAmount = value;
        }
    }
}
