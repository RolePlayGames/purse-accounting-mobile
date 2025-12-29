namespace PurseAccountinng.Mobile.Presentation.Components;

public partial class ToastView : ContentView
{
    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        nameof(Text), typeof(string), typeof(ToastView), string.Empty);

    public static new readonly BindableProperty BackgroundColorProperty = BindableProperty.Create(
        nameof(BackgroundColor), typeof(Color), typeof(ToastView), Microsoft.Maui.Graphics.Colors.Gray);

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public new Color BackgroundColor
    {
        get => (Color)GetValue(BackgroundColorProperty);
        set => SetValue(BackgroundColorProperty, value);
    }

    public ToastView()
    {
        InitializeComponent();
    }

    public async Task ShowAsync(uint durationMs = 3000)
    {
        IsVisible = true;

        var rootLayout = GetRootLayoutFromCurrentPage();

        if (rootLayout is null)
        {
            IsVisible = false;
            return;
        }

        if (rootLayout is Grid)
        {
            Grid.SetRow(this, 0);
            Grid.SetRowSpan(this, int.MaxValue);
        }

        rootLayout.Add(this);

        HorizontalOptions = LayoutOptions.Fill;
        VerticalOptions = LayoutOptions.End;
        Margin = new Thickness(3, 0, 3, 156);

        await this.FadeTo(1, 150);
        await Task.Delay((int)durationMs);
        await this.FadeTo(0, 200);

        rootLayout.Remove(this);
        IsVisible = false;
    }

    private static Layout? GetRootLayoutFromCurrentPage()
    {
        var window = Application.Current?.Windows?.FirstOrDefault();
        if (window?.Page is not Page currentPage)
            return null;

        // Разворачиваем NavigationPage, если есть
        var currentContentPage = currentPage switch
        {
            ContentPage contentPage => contentPage,
            NavigationPage navigationPage => navigationPage.CurrentPage as ContentPage,
            _ => null,
        };

        if (currentContentPage?.Content is Layout layout)
            return layout;

        // Если Content не Layout — оборачиваем в Grid
        if (currentContentPage?.Content is not null)
        {
            var grid = new Grid { Children = { currentContentPage.Content } };
            currentContentPage.Content = grid;
            return grid;
        }

        return null;
    }
}
