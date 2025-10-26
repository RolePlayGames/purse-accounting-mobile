namespace PurseAccountinng.Mobile.Presentation.Components;

public partial class LinkLabel : ContentView
{
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(
            nameof(Text),
            typeof(string),
            typeof(LinkLabel),
            string.Empty);

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly new BindableProperty HorizontalOptionsProperty =
        BindableProperty.Create(
            nameof(HorizontalOptions),
            typeof(LayoutOptions),
            typeof(LinkLabel),
            LayoutOptions.Start);

    public new LayoutOptions HorizontalOptions
    {
        get => (LayoutOptions)GetValue(HorizontalOptionsProperty);
        set => SetValue(HorizontalOptionsProperty, value);
    }

    public event EventHandler? Tapped;

    public LinkLabel()
    {
        InitializeComponent();

        var tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += OnLabelTapped;
        TextLabel.GestureRecognizers.Add(tapGesture);

        TextLabel.SetBinding(Label.TextProperty, new Binding(nameof(Text), source: this));
        TextLabel.SetBinding(Label.HorizontalOptionsProperty, new Binding(nameof(HorizontalOptions), source: this));
    }

    private void OnLabelTapped(object? sender, EventArgs e)
    {
        Tapped?.Invoke(this, e);
    }
}
