namespace PurseAccountinng.Mobile.Presentation.Components;

public partial class PasswordField : ContentView
{
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(
            nameof(Text),
            typeof(string),
            typeof(PasswordField),
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                if (bindable is PasswordField field && field.FieldEntry.Text != (string?)newValue)
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
            typeof(PasswordField),
            string.Empty);

    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    public PasswordField()
    {
        InitializeComponent();

        FieldEntry.SetBinding(Entry.TextProperty, new Binding(nameof(Text), source: this));
        FieldEntry.SetBinding(Entry.PlaceholderProperty, new Binding(nameof(Placeholder), source: this));
    }

    private void OnToggleVisibilityClicked(object sender, EventArgs e)
    {
        FieldEntry.IsPassword = !FieldEntry.IsPassword;

        ToggleVisibilityButton.Source = FieldEntry.IsPassword
            ? "visibility_outlined.png"
            : "visibility_off_outlined.png";
    }
}
