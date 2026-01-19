using PurseAccountinng.Mobile.Presentation.Extensions;
using System.Globalization;

namespace PurseAccountinng.Mobile.Presentation.Components.Categories;

internal class BoolToCategoryBackgroundConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not bool isSelected)
            return Brush.Transparent;

        if (isSelected)
            return new SolidColorBrush(App.Current?.Resources.GetColor("LightBlue"));

        return Brush.Transparent;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
