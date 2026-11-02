namespace PurseAccountinng.Mobile.Presentation.Components.Icons;

public partial class AutoPlannedSettingIcon : ContentView
{
    public static readonly BindableProperty IsActiveProperty =
        BindableProperty.Create(nameof(IsActive), typeof(bool), typeof(AutoPlannedSettingIcon), defaultValue: true);

    public bool IsActive
    {
        get => (bool)GetValue(IsActiveProperty);
        set => SetValue(IsActiveProperty, value);
    }

    public AutoPlannedSettingIcon()
    {
        InitializeComponent();
    }
}
