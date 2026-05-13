namespace PurseAccountinng.Mobile.Presentation.Components.Fields;

internal static class VisualElementExtensions
{
    public static void Set(this VisualElement element, VisualSettings settings)
    {
        element.Opacity = settings.Opacity;
        element.TranslationY = settings.TranslationY;
        element.Scale = settings.Scale;
    }
}
