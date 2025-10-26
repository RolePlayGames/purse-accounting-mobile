using PurseAccountinng.Mobile.Presentation.Extensions;
using System.Windows.Input;

namespace PurseAccountinng.Mobile.Presentation.Components;

public partial class PrimaryButton : ContentView
{
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(
            nameof(Text),
            typeof(string),
            typeof(PrimaryButton),
            "Action");

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(
            nameof(Command),
            typeof(ICommand),
            typeof(PrimaryButton));

    public ICommand? Command
    {
        get => (ICommand?)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public static readonly new BindableProperty IsEnabledProperty =
        BindableProperty.Create(
            nameof(IsEnabled),
            typeof(bool),
            typeof(PrimaryButton),
            true,
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                if (bindable is PrimaryButton button)
                {
                    button.MainButton.IsEnabled = (bool)newValue;
                    button.OnIsEnabledChanged();
                }
            });

    public new bool IsEnabled
    {
        get => (bool)GetValue(IsEnabledProperty);
        set => SetValue(IsEnabledProperty, value);
    }

    private readonly Color? _activeBackgroundColor;
    private readonly Color? _activeTextColor;
    private readonly Color? _inactiveBackgroundColor;
    private readonly Color? _inactiveTextColor;
    private readonly Color? _pressedColor;

    public PrimaryButton()
    {
        InitializeComponent();

        _activeBackgroundColor = Application.Current?.Resources.GetColor("Purple");
        _activeTextColor = Application.Current?.Resources.GetColor("WorkBackground");
        _inactiveBackgroundColor = Application.Current?.Resources.GetColor("InactiveElementFill");
        _inactiveTextColor = Application.Current?.Resources.GetColor("InactiveElementText");
        _pressedColor = Application.Current?.Resources.GetColor("Blue");

        MainButton.SetBinding(Button.TextProperty, new Binding(nameof(Text), source: this));
        MainButton.SetBinding(Button.CommandProperty, new Binding(nameof(Command), source: this));
        MainButton.SetBinding(Button.IsEnabledProperty, new Binding(nameof(IsEnabled), source: this));

        PropertyChanged += OnIsEnabledChanged;
        OnIsEnabledChanged();
    }

    private void OnIsEnabledChanged(object? sender = null, System.ComponentModel.PropertyChangedEventArgs? e = null)
    {
        if (MainButton.IsEnabled)
        {
            MainButton.BackgroundColor = _activeBackgroundColor;
            MainButton.TextColor = _activeTextColor;
        }
        else
        {
            MainButton.BackgroundColor = _inactiveBackgroundColor;
            MainButton.TextColor = _inactiveTextColor;
        }
    }

    private void OnButtonPressed(object sender, EventArgs e)
    {
        if (MainButton.IsEnabled)
        {
            MainButton.BackgroundColor = _pressedColor;
        }
    }

    private void OnButtonReleased(object sender, EventArgs e)
    {
        OnIsEnabledChanged();
    }
}
