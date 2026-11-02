namespace PurseAccountinng.Mobile.Presentation.Components.Icons;

public partial class AutoPlannedSettingIcon : ContentView
{
    public static readonly BindableProperty IsActiveProperty =
        BindableProperty.Create(nameof(IsActive), typeof(bool), typeof(AutoPlannedSettingIcon), defaultValue: true);

    /// <summary>
    /// Icon color. Derived from <see cref="IsActive"/> by default, but can be set from the outside.
    /// </summary>
    public static readonly BindableProperty ColorProperty =
        BindableProperty.Create(nameof(Color), typeof(Color), typeof(AutoPlannedSettingIcon));

    public bool IsActive
    {
        get => (bool)GetValue(IsActiveProperty);
        set => SetValue(IsActiveProperty, value);
    }

    public Color Color
    {
        get => (Color)GetValue(ColorProperty);
        set => SetValue(ColorProperty, value);
    }

    public AutoPlannedSettingIcon()
    {
        InitializeComponent();
    }
}
