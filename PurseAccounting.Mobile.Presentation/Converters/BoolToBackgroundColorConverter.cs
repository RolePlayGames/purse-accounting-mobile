using System.Globalization;

namespace PurseAccountinng.Mobile.Presentation.Converters;

public class BoolToBackgroundColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (bool)value
            ? Application.Current?.Resources["Purple"] as Color ?? Microsoft.Maui.Graphics.Colors.Purple
            : Application.Current?.Resources["InactiveElementFill"] as Color ?? Microsoft.Maui.Graphics.Colors.LightGray;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        throw new NotImplementedException();
}
