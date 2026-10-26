namespace PurseAccountinng.Mobile.Presentation.Components;

public partial class IosStyleSwitch : ContentView
{
    private const uint ToggleAnimationDuration = 150;

    public static readonly BindableProperty IsToggledProperty = BindableProperty.Create(
        nameof(IsToggled), typeof(bool), typeof(IosStyleSwitch), false, BindingMode.TwoWay,
        propertyChanged: OnIsToggledPropertyChanged);

    public static readonly BindableProperty DescriptionProperty = BindableProperty.Create(
        nameof(Description), typeof(string), typeof(IosStyleSwitch), string.Empty);

    public bool IsToggled
    {
        get => (bool)GetValue(IsToggledProperty);
        set => SetValue(IsToggledProperty, value);
    }

    public string Description
    {
        get => (string)GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }

    public event EventHandler<bool>? Toggled;

    public IosStyleSwitch()
    {
        InitializeComponent();

        var tapGestureRecognizer = new TapGestureRecognizer
        {
            Command = new Command(() => IsToggled = !IsToggled)
        };

        SwitchTrack.GestureRecognizers.Add(tapGestureRecognizer);
        SwitchThumb.GestureRecognizers.Add(tapGestureRecognizer);
    }

    private static void OnIsToggledPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is IosStyleSwitch sw)
        {
            sw.UpdateThumbPosition((bool)newValue);
        }
    }

    private void UpdateThumbPosition(bool isToggled)
    {
        // Proportional position: 0 -> thumb on the left, 1 -> thumb on the right.
        var targetX = isToggled ? 1d : 0d;

        SwitchThumb.AbortAnimation(nameof(SwitchThumb));
        SwitchThumb.Animate(
            nameof(SwitchThumb),
            () => AbsoluteLayout.GetLayoutBounds(SwitchThumb),
            current => new Rect(targetX, 0.5, 32, 32),
            length: ToggleAnimationDuration,
            easing: Easing.CubicOut);

        Toggled?.Invoke(this, isToggled);
    }
}
