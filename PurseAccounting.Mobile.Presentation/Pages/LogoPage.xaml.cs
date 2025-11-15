using PurseAccounting.Mobile.Application.Accounting;
using PurseAccountinng.Mobile.Presentation.Pages.Authorized;
using PurseAccountinng.Mobile.Presentation.Pages.Unauthorized.Login;

namespace PurseAccountinng.Mobile.Presentation.Pages;

public partial class LogoPage : ContentPage
{
    private const int _maxAttempts = 20;

    private readonly IServiceProvider _serviceProvider;
    private readonly IAccountingService _accountingService;

    public LogoPage(IServiceProvider serviceProvider, IAccountingService accountingService)
    {
        _serviceProvider = serviceProvider;
        _accountingService = accountingService;

        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        CheckAuthorize().ConfigureAwait(false);
    }

    private async Task CheckAuthorize()
    {
        await Task.Delay(100);

        var attemptCount = 0;

        while (attemptCount <= _maxAttempts)
        {
            attemptCount++;

            try
            {
                var isSucceed = await _accountingService.LoadAccount();

                if (isSucceed)
                {
                    await NavigateToMainPageAsync(true).ConfigureAwait(false);
                    return;
                }
                else
                {
                    await NavigateToMainPageAsync(false).ConfigureAwait(false);
                    return;
                }
            }
            catch (TaskCanceledException)
            {
                await HandleError("Сервер долго не отвечает", attemptCount);
            }
            catch
            {
                await HandleError("Ошибка подключения", attemptCount);
            }

            await Task.Delay(100);
        }

        await DisplayFinalError();
    }

    private async Task HandleError(string message, int attemptCount)
    {
        if (attemptCount <= 1)
            return;

        var displayMessage = $"{message}. Пробуем ещё раз...";

        await MainThread.InvokeOnMainThreadAsync(() =>
        {
            ErrorMessageLabel.Text = displayMessage;
            ErrorMessageLabel.IsVisible = true;
        }).ConfigureAwait(false);

        await Task.Delay(500);
    }

    private async Task DisplayFinalError()
    {
        await MainThread.InvokeOnMainThreadAsync(() =>
        {
            ErrorMessageLabel.Text = "Не удаётся подключиться к серверу. Проверьте интернет и перезапустите приложение.";
            ErrorMessageLabel.IsVisible = true;
        }).ConfigureAwait(false);
    }

    private async Task NavigateToMainPageAsync(bool isAuthorized)
    {
        await MainThread.InvokeOnMainThreadAsync(() =>
        {
            ErrorMessageLabel.IsVisible = false;

            if (Application.Current?.Windows[0].Page is not null)
            {
                ContentPage page = isAuthorized
                    ? _serviceProvider.GetRequiredService<AuthorizedPage>()
                    : _serviceProvider.GetRequiredService<LoginPage>();

                Application.Current.Windows[0].Page = new NavigationPage(page);
            }
        }).ConfigureAwait(false);
    }
}
