using PurseAccountinng.Mobile.Presentation.Extensions;
using System.Globalization;

namespace PurseAccountinng.Mobile.Presentation.Converters;

public class BoolToTextColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (bool)value
            ? Application.Current?.Resources.GetColor("WorkBackground") ?? Microsoft.Maui.Graphics.Colors.White
            : Application.Current?.Resources.GetColor("DarkPurple") ?? Microsoft.Maui.Graphics.Colors.Purple;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        throw new NotImplementedException();
}
