using PurseAccountinng.Mobile.Presentation.Extensions;
using System.Globalization;

namespace PurseAccountinng.Mobile.Presentation.Components.Icons;

public class AutoPlannedSettingIconStateToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (bool)value
            ? Application.Current?.Resources.GetColor("DarkPurple") ?? Microsoft.Maui.Graphics.Colors.Purple
            : Application.Current?.Resources.GetColor("InactiveElementText") ?? Microsoft.Maui.Graphics.Colors.Gray;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        throw new NotImplementedException();
}
