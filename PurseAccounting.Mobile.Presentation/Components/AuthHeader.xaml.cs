namespace PurseAccountinng.Mobile.Presentation.Components;

public partial class AuthHeader : ContentView
{
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(
            nameof(Text),
            typeof(string),
            typeof(AuthHeader),
            string.Empty);

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public AuthHeader()
    {
        InitializeComponent();

        HeaderText.SetBinding(Label.TextProperty, new Binding(nameof(Text), source: this));
    }
}
