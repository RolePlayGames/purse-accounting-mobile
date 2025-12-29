namespace PurseAccountinng.Mobile.Presentation.Pages.Authorized.Widgets;

public partial class TomorrowAmountWidget : ContentView
{
    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        nameof(Text),
        typeof(string),
        typeof(TomorrowAmountWidget),
        string.Empty,
        propertyChanged: OnTextChanged);

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    private static void OnTextChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TomorrowAmountWidget widget && widget.Content is Border border && border.Content is Label label)
        {
            label.Text = (string)newValue;
        }
    }

    public TomorrowAmountWidget()
    {
        InitializeComponent();
    }
}
