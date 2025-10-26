namespace PurseAccountinng.Mobile.Presentation.Extensions;

public static class ReasourceDictionaryExtensions
{
    private static readonly Color _defaultColor = Color.FromArgb("FFFFFF");

    public static Color GetColor(this ResourceDictionary resources, string colorName)
    {
        return resources.TryGetValue(colorName, out var resource) && resource is Color color
            ? color
            : _defaultColor;
    }
}
