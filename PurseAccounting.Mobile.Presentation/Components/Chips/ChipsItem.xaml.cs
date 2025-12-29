namespace PurseAccountinng.Mobile.Presentation.Components.Chips;

public partial class ChipsItem : ContentView
{
    public static readonly BindableProperty IsActiveProperty = BindableProperty.Create(
        nameof(IsActive), typeof(bool), typeof(ChipsItem), false);

    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        nameof(Text), typeof(string), typeof(ChipsItem), string.Empty);

    public static readonly BindableProperty ValueProperty = BindableProperty.Create(
        nameof(Value), typeof(object), typeof(ChipsItem), null);

    public bool IsActive
    {
        get => (bool)GetValue(IsActiveProperty);
        set => SetValue(IsActiveProperty, value);
    }

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public object Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public ChipsItem()
    {
        InitializeComponent();

        ChipBorder.GestureRecognizers.Add(new TapGestureRecognizer
        {
            Command = new Command(() => ChipTapped?.Invoke(this, EventArgs.Empty))
        });
    }

    public event EventHandler? ChipTapped;
}
