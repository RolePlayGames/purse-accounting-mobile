using System.Globalization;

namespace PurseAccountinng.Mobile.Presentation.Components;

/// <summary>
/// Returns the switch track color: "DarkPurple" when the switch is on, "InactiveElementFill" when it is off.
/// </summary>
public class BoolToSwitchTrackColorConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var key = value is true ? "DarkPurple" : "InactiveElementFill";
        return (Color)Application.Current!.Resources[key];
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
