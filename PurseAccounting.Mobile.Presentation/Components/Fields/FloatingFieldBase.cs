namespace PurseAccountinng.Mobile.Presentation.Components.Fields;

public abstract class FloatingFieldBase : ContentView
{
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(
            nameof(Text),
            typeof(string),
            typeof(FloatingFieldBase),
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: OnTextChanged);

    public static readonly BindableProperty PlaceholderProperty =
        BindableProperty.Create(
            nameof(Placeholder),
            typeof(string),
            typeof(FloatingFieldBase),
            string.Empty);

    private static readonly VisualSettings _invisibleHeaderSetting = new() { Opacity = 0, TranslationY = 5, Scale = 0.95 };
    private static readonly VisualSettings _visibleHeaderSetting = new() { Opacity = 1, TranslationY = 0, Scale = 1 };

    public string? Text
    {
        get => (string?)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    protected Entry? FieldEntry { get; private set; }

    protected Label? TitleLabel { get; private set; }

    protected FloatingFieldBase()
    {
    }

    /// <summary>
    /// Initializes bindings.
    /// Initialize in inheritance AFTER InitializeComponent() !!!
    /// </summary>
    /// <param name="entry">Text field</param>
    /// <param name="label">Header label</param>
    protected void InitializeFloatingField(Entry entry, Label label)
    {
        FieldEntry = entry;
        TitleLabel = label;

        FieldEntry.SetBinding(Entry.TextProperty, new Binding(nameof(Text), source: this));
        FieldEntry.SetBinding(Entry.PlaceholderProperty, new Binding(nameof(Placeholder), source: this));
        TitleLabel.SetBinding(Label.TextProperty, new Binding(nameof(Placeholder), source: this));

        FieldEntry.Focused += OnEntryFocused;
        FieldEntry.Unfocused += OnEntryUnfocused;
        FieldEntry.TextChanged += OnEntryTextChanged;

        UpdateTitleVisibility();
    }

    protected async void UpdateTitleVisibility()
    {
        if (TitleLabel == null || FieldEntry == null)
            return;

        var shouldShow = !string.IsNullOrWhiteSpace(FieldEntry.Text) && FieldEntry.IsFocused;
        var targetSetting = shouldShow ? _visibleHeaderSetting : _invisibleHeaderSetting;

        await Task.WhenAll(
            TitleLabel.FadeTo(targetSetting.Opacity, 300, Easing.CubicInOut),
            TitleLabel.TranslateTo(0, targetSetting.TranslationY, 300, Easing.CubicInOut),
            TitleLabel.ScaleTo(targetSetting.Scale, 300, Easing.CubicInOut))
            .ConfigureAwait(false);

        if (!shouldShow)
        {
            TitleLabel.Set(targetSetting);
        }
    }

    private static void OnTextChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is FloatingFieldBase field
            && field.FieldEntry is not null
            && newValue is string stringValue
            && field.FieldEntry.Text != stringValue)
        {
            field.FieldEntry.Text = stringValue;
        }
    }

    private void OnEntryFocused(object? sender, FocusEventArgs e) => UpdateTitleVisibility();

    private void OnEntryUnfocused(object? sender, FocusEventArgs e) => UpdateTitleVisibility();

    private void OnEntryTextChanged(object? sender, TextChangedEventArgs e) => UpdateTitleVisibility();
}
