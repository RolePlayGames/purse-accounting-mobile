namespace PurseAccountinng.Mobile.Presentation.Components;

public partial class FooterLink : ContentView
{
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(
            nameof(Text),
            typeof(string),
            typeof(FooterLink),
            string.Empty);

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public event EventHandler? Tapped;

    public FooterLink()
    {
        InitializeComponent();

        var tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += OnLabelTapped;
        TextLabel.GestureRecognizers.Add(tapGesture);

        TextSpan.SetBinding(Span.TextProperty, new Binding(nameof(Text), source: this));
    }

    private void OnLabelTapped(object? sender, EventArgs e)
    {
        Tapped?.Invoke(this, e);
    }
}
