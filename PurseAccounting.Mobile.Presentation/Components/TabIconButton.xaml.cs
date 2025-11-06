using System.Windows.Input;

namespace PurseAccountinng.Mobile.Presentation.Components;

public partial class TabIconButton : ContentView
{
    public event EventHandler? Clicked;

    public static readonly BindableProperty IsActiveProperty =
        BindableProperty.Create(
            nameof(IsActive),
            typeof(bool),
            typeof(TabIconButton),
            false,
            propertyChanged: OnAnySourceChanged);

    public static readonly BindableProperty ActiveSourceProperty =
        BindableProperty.Create(
            nameof(ActiveSource),
            typeof(ImageSource),
            typeof(TabIconButton),
            propertyChanged: OnAnySourceChanged);

    public static readonly BindableProperty InactiveSourceProperty =
        BindableProperty.Create(
            nameof(InactiveSource),
            typeof(ImageSource),
            typeof(TabIconButton),
            propertyChanged: OnAnySourceChanged);

    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(
            nameof(Command),
            typeof(ICommand),
            typeof(TabIconButton));

    public bool IsActive
    {
        get => (bool)GetValue(IsActiveProperty);
        set => SetValue(IsActiveProperty, value);
    }

    public ImageSource ActiveSource
    {
        get => (ImageSource)GetValue(ActiveSourceProperty);
        set => SetValue(ActiveSourceProperty, value);
    }

    public ImageSource InactiveSource
    {
        get => (ImageSource)GetValue(InactiveSourceProperty);
        set => SetValue(InactiveSourceProperty, value);
    }

    public ICommand? Command
    {
        get => (ICommand?)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public TabIconButton()
    {
        InitializeComponent();
        // Начальное обновление — на случай, если свойства уже заданы
        UpdateImageSource();
    }

    private static void OnAnySourceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TabIconButton control)
        {
            control.UpdateImageSource();
        }
    }

    private void UpdateImageSource()
    {
        var source = IsActive ? ActiveSource : InactiveSource;
        ImageButton.Source = source; // может быть null — это нормально
    }

    private void OnImageButtonClicked(object sender, EventArgs e)
    {
        Command?.Execute(null);
        Clicked?.Invoke(this, EventArgs.Empty);
    }
}
