using PurseAccountinng.Mobile.Presentation.Extensions;
using System.Windows.Input;

namespace PurseAccountinng.Mobile.Presentation.Components;

public partial class GoogleSignInButton : ContentView
{
    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(
            nameof(Command),
            typeof(ICommand),
            typeof(GoogleSignInButton));

    public ICommand? Command
    {
        get => (ICommand?)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public static readonly new BindableProperty IsEnabledProperty =
        BindableProperty.Create(
            nameof(IsEnabled),
            typeof(bool),
            typeof(GoogleSignInButton),
            true,
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                if (bindable is GoogleSignInButton button)
                {
                    button.UpdateAppearance();
                }
            });

    public new bool IsEnabled
    {
        get => (bool)GetValue(IsEnabledProperty);
        set => SetValue(IsEnabledProperty, value);
    }

    private readonly Color? _enabledBackgroundColor;
    private readonly Color? _disabledBackgroundColor;
    private readonly Color? _pressedBackgroundColor;

    public GoogleSignInButton()
    {
        InitializeComponent();

        _enabledBackgroundColor = Application.Current?.Resources.GetColor("WorkBackground");
        _disabledBackgroundColor = Application.Current?.Resources.GetColor("InactiveElementFill");
        _pressedBackgroundColor = Application.Current?.Resources.GetColor("InactiveElementFill");

        var tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += OnButtonTapped;
        GoogleButtonBorder.GestureRecognizers.Add(tapGesture);

        UpdateAppearance();
    }

    private async void OnButtonTapped(object? sender, EventArgs e)
    {
        if (!IsEnabled)
            return;

        GoogleButtonBorder.BackgroundColor = _pressedBackgroundColor;

        Command?.Execute(null);

        await Task.Delay(100);
        UpdateAppearance();
    }

    private void UpdateAppearance()
    {
        GoogleButtonBorder.BackgroundColor = IsEnabled
            ? _enabledBackgroundColor
            : _disabledBackgroundColor;
    }
}
