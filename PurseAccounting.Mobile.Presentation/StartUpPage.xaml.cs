namespace PurseAccountinng.Mobile.Presentation;

/// <summary>
/// Application startup page
/// </summary>
public partial class StartUpPage : ContentPage
{
    private const string _loginPagePath = "//LoginPage";
    private const string _accountingPagePath = "//AccountingPage";

    private readonly CancellationTokenSource _timeoutCancellationTokenSource = new(TimeSpan.FromSeconds(3));

    public StartUpPage()
    {
        InitializeComponent();
        Shell.SetNavBarIsVisible(this, false);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CheckAuthenticationAndRedirect();
    }

    private async Task CheckAuthenticationAndRedirect()
    {
        try
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("https://purse-accounting.ru/api/accounting/account", _timeoutCancellationTokenSource.Token);

            var targetRoute = response.IsSuccessStatusCode
                ? _accountingPagePath
                : _loginPagePath;

            await Shell.Current.GoToAsync(targetRoute);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Auth check failed: {ex}");
            await Shell.Current.GoToAsync(_loginPagePath);
        }
    }
}
