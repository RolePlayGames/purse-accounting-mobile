using PurseAccountinng.Mobile.Presentation.Extensions;
using System.Globalization;

namespace PurseAccountinng.Mobile.Presentation.Components.Categories;

public class BoolToCategoryTextColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (bool)value
            ? Application.Current?.Resources.GetColor("DarkPurple") ?? Microsoft.Maui.Graphics.Colors.Purple
            : Application.Current?.Resources.GetColor("Gray2") ?? Microsoft.Maui.Graphics.Colors.DarkGray;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        throw new NotImplementedException();
}
