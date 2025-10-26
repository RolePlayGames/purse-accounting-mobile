using static System.Net.Mime.MediaTypeNames;

namespace PurseAccountinng.Mobile.Presentation.Components;

public partial class TextField : ContentView
{
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(
            nameof(Text),
            typeof(string),
            typeof(TextField),
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                if (bindable is TextField field && field.FieldEntry.Text != (string?)newValue)
                {
                    field.FieldEntry.Text = (string?)newValue;
                }
            });

    public string? Text
    {
        get => (string?)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly BindableProperty PlaceholderProperty =
        BindableProperty.Create(
            nameof(Placeholder),
            typeof(string),
            typeof(TextField),
            string.Empty);

    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }
    
    public TextField()
	{
		InitializeComponent();

        FieldEntry.SetBinding(Entry.TextProperty, new Binding(nameof(Text), source: this));
        FieldEntry.SetBinding(Entry.PlaceholderProperty, new Binding(nameof(Placeholder), source: this));
    }
}
