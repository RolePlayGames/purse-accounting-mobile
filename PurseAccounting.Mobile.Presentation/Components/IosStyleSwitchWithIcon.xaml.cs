namespace PurseAccountinng.Mobile.Presentation.Components;

public partial class IosStyleSwitchWithIcon : ContentView
{
    public static readonly BindableProperty IsToggledProperty = BindableProperty.Create(
        nameof(IsToggled), typeof(bool), typeof(IosStyleSwitchWithIcon), false, BindingMode.TwoWay,
        propertyChanged: OnIsToggledPropertyChanged);

    public static readonly BindableProperty DescriptionProperty = BindableProperty.Create(
        nameof(Description), typeof(string), typeof(IosStyleSwitchWithIcon), string.Empty);

    private const uint _toggleAnimationDurationMilleseconds = 200;
    private const double _thumbPadding = 2.0;
    private const double _targetTranslationX = 66 - 32 - _thumbPadding; // left position = 2, right position = 66 (width) - 32 (circle width) - 2 (padding) = 32

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

    public IosStyleSwitchWithIcon()
    {
        InitializeComponent();

        UpdateThumbPosition(IsToggled, animate: false); // initialize first position

        var tapGestureRecognizer = new TapGestureRecognizer
        {
            Command = new Command(() => IsToggled = !IsToggled),
        };

        SwitchTrack.GestureRecognizers.Add(tapGestureRecognizer);
        SwitchThumb.GestureRecognizers.Add(tapGestureRecognizer);
    }

    private static void OnIsToggledPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is IosStyleSwitchWithIcon sw)
        {
            sw.UpdateThumbPosition((bool)newValue, animate: true);
            sw.Toggled?.Invoke(sw, (bool)newValue);
        }
    }

    private void UpdateThumbPosition(bool isToggled, bool animate)
    {
        var targetX = isToggled ? _targetTranslationX : _thumbPadding;

        SwitchThumb.AbortAnimation(nameof(SwitchThumb));

        if (animate)
        {
            SwitchThumb.Animate(
                nameof(SwitchThumb),
                progress =>
                {
                    var currentX = SwitchThumb.TranslationX + ((targetX - SwitchThumb.TranslationX) * progress);
                    SwitchThumb.TranslationX = currentX;
                },
                length: _toggleAnimationDurationMilleseconds,
                easing: Easing.CubicInOut);
        }
        else
        {
            SwitchThumb.TranslationX = targetX;
        }
    }
}
