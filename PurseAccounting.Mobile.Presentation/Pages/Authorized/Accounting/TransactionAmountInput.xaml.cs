using PurseAccountinng.Mobile.Presentation.Extensions;
using PurseAccountinng.Mobile.Presentation.Services;
using System.Globalization;
using System.Windows.Input;

namespace PurseAccountinng.Mobile.Presentation.Pages.Authorized.Accounting;

public partial class TransactionAmountInput : ContentView
{
    public static readonly BindableProperty SubmitCommandProperty
        = BindableProperty.Create(nameof(SubmitCommand), typeof(ICommand), typeof(TransactionAmountInput), null);

    public static readonly BindableProperty AmountProperty
        = BindableProperty.Create(
            propertyName: nameof(Amount),
            returnType: typeof(int?),
            declaringType: typeof(TransactionAmountInput),
            defaultValue: 0,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: OnAmountChanged);

    public static readonly BindableProperty IsSubmitEnabledProperty = BindableProperty.Create(
        nameof(IsSubmitEnabled),
        typeof(bool),
        typeof(TransactionAmountInput),
        true,
        propertyChanged: OnIsSubmitEnabledChanged);

    private const double _pressedScale = 0.95;
    private const uint _duration = 80;

    public ICommand? SubmitCommand
    {
        get => (ICommand?)GetValue(SubmitCommandProperty);
        set => SetValue(SubmitCommandProperty, value);
    }

    public int? Amount
    {
        get => (int?)GetValue(AmountProperty);
        set => SetValue(AmountProperty, value);
    }

    public bool IsSubmitEnabled
    {
        get => (bool)GetValue(IsSubmitEnabledProperty);
        set => SetValue(IsSubmitEnabledProperty, value);
    }

    private bool _isAnimating = false;

    public TransactionAmountInput()
    {
        InitializeComponent();
    }

    private static void OnAmountChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not TransactionAmountInput control)
            return;

        var amountInCents = (int?)newValue;

        var text = amountInCents.HasValue switch
        {
            true => (amountInCents.Value / 100m).ToString("0.##", CultureInfo.CreateSpecificCulture("ru-RU")),
            false => string.Empty,
        };

        if (control.AmountEntry.Text != text)
            control.AmountEntry.Text = text;
    }

    private static void OnIsSubmitEnabledChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TransactionAmountInput control)
        {
            control.UpdateButtonState();
        }
    }

    private void OnAmountTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (sender is not Entry entry)
            return;

        var input = e.NewTextValue ?? string.Empty;
        var sanitized = AmountParser.FilterInput(input);

        if (sanitized != input)
        {
            entry.TextChanged -= OnAmountTextChanged;
            entry.Text = sanitized;
            entry.TextChanged += OnAmountTextChanged;
            entry.CursorPosition = entry.Text.Length;
        }

        if (AmountParser.TryParseToCents(sanitized, out var cents) && Amount != cents)
            Amount = cents;
        else if (sanitized == string.Empty)
            Amount = null;
    }

    private async void OnSubmitTapped(object sender, TappedEventArgs e)
    {
        if (!IsSubmitEnabled || Application.Current is null || _isAnimating)
            return;

        _isAnimating = false;

        var originalScale = SubmitButton.Scale;
        var originalBrush = SubmitButton.Background;
        var pressedBrush = new SolidColorBrush(Application.Current.Resources.GetColor("Blue"));

        try
        {
            var scaleDown = SubmitButton.ScaleTo(_pressedScale, _duration / 2, Easing.CubicOut);

            SubmitButton.Background = pressedBrush;

            await scaleDown;
            await Task.Delay(50);

            SubmitButton.Background = originalBrush;

            if (SubmitCommand is not null && SubmitCommand.CanExecute(null))
                SubmitCommand.Execute(null);

            var scaleUp = SubmitButton.ScaleTo(originalScale, _duration / 2, Easing.CubicOut);

            await scaleUp;
        }
        catch
        {
            SubmitButton.Scale = originalScale;
        }
        finally
        {
            _isAnimating = false;
        }
    }

    private void UpdateButtonState()
    {
        if (Application.Current is null)
            return;

        if (IsSubmitEnabled)
        {
            SubmitButton.Background = new SolidColorBrush(Application.Current.Resources.GetColor("Purple"));
            SubmitIcon.Fill = new SolidColorBrush(Application.Current.Resources.GetColor("WorkBackground"));
        }
        else
        {
            SubmitButton.Background = new SolidColorBrush(Application.Current.Resources.GetColor("InactiveElementFill"));
            SubmitIcon.Fill = new SolidColorBrush(Application.Current.Resources.GetColor("InactiveElementText"));
        }
    }
}
