using System.Windows.Input;

namespace PurseAccountinng.Mobile.Presentation.Components;

public partial class MenuItem : ContentView
{
    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(nameof(Title), typeof(string), typeof(MenuItem), string.Empty);

    public static readonly BindableProperty IconSourceProperty =
        BindableProperty.Create(nameof(IconSource), typeof(ImageSource), typeof(MenuItem), null);

    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(MenuItem), null);

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public ImageSource IconSource
    {
        get => (ImageSource)GetValue(IconSourceProperty);
        set => SetValue(IconSourceProperty, value);
    }

    public event EventHandler? Clicked;

    public MenuItem()
    {
        InitializeComponent();

        // Привязки
        TitleLabel.SetBinding(Label.TextProperty, new Binding(nameof(Title), source: this));
        IconImage.SetBinding(Image.SourceProperty, new Binding(nameof(IconSource), source: this));

        // Обработка нажатия
        var tap = new TapGestureRecognizer();
        tap.Tapped += (s, e) =>
        {
            Clicked?.Invoke(this, EventArgs.Empty);
        };
        RootGrid.GestureRecognizers.Add(tap);
    }
}
