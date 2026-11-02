using PurseAccountinng.Mobile.Presentation.Extensions;

namespace PurseAccountinng.Mobile.Presentation.Components.Icons;

public partial class AutoPlannedSettingIcon : ContentView
{
    public static readonly BindableProperty IsActiveProperty =
        BindableProperty.Create(
            nameof(IsActive),
            typeof(bool),
            typeof(AutoPlannedSettingIcon),
            defaultValue: true,
            propertyChanged: OnIsActivePropertyChanged);

    /// <summary>
    /// Icon color. Derived from <see cref="IsActive"/> by default (DarkPurple when active,
    /// InactiveElementText otherwise), but can be set from the outside.
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
        UpdateColorFromState();
    }

    private static void OnIsActivePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        ((AutoPlannedSettingIcon)bindable).UpdateColorFromState();
    }

    private void UpdateColorFromState()
    {
        var resources = Application.Current?.Resources;

        if (resources is null)
            return;

        Color = IsActive
            ? resources.GetColor("DarkPurple")
            : resources.GetColor("InactiveElementText");
    }
}
