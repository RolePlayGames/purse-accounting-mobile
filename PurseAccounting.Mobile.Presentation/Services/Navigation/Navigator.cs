namespace PurseAccountinng.Mobile.Presentation.Services.Navigation;

internal class Navigator : INavigator
{
    private readonly IServiceProvider _serviceProvider;

    public Navigator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task ChangePageTo<TContentPage>(CancellationToken ct) where TContentPage : ContentPage
    {
        await MainThread.InvokeOnMainThreadAsync(() =>
        {
            if (Application.Current?.Windows[0].Page is not null)
            {
                var page = _serviceProvider.GetRequiredService<TContentPage>();
                Application.Current.Windows[0].Page = new NavigationPage(page);
            }
        }).ConfigureAwait(false);
    }
}
