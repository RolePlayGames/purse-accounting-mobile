using PurseAccounting.Mobile.Application.Accounting;
using PurseAccounting.Mobile.Infrastructure.HttpClientInitializers;
using PurseAccountinng.Mobile.Presentation.Pages.Authorized;
using PurseAccountinng.Mobile.Presentation.Pages.Unauthorized.Login;
using PurseAccountinng.Mobile.Presentation.Services.Navigation;

namespace PurseAccountinng.Mobile.Presentation.Pages;

public partial class LogoPage : ContentPage
{
    private const int _maxAttempts = 20;
    private static readonly TimeSpan _requestTimeout = TimeSpan.FromSeconds(2);

    private readonly IAccountingService _accountingService;
    private readonly INavigator _navigator;
    private readonly IHttpClientInitializer _httpClientInitializer;

    public LogoPage(IAccountingService accountingService, INavigator navigator, IHttpClientInitializer httpClientInitializer)
    {
        _accountingService = accountingService;
        _navigator = navigator;
        _httpClientInitializer = httpClientInitializer;

        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        CheckAuthorize().ConfigureAwait(false);
    }

    private async Task CheckAuthorize()
    {
        await _httpClientInitializer.Initialize();

        var attemptCount = 0;

        while (attemptCount++ < _maxAttempts)
        {
            try
            {
                using var timeoutTokenSource = new CancellationTokenSource(_requestTimeout);
                var account = await _accountingService.LoadAccount(timeoutTokenSource.Token);

                await NavigateToMainPageAsync(account is not null).ConfigureAwait(false);

                return;
            }
            catch (TaskCanceledException ex)
            {
                await HandleError("Сервер долго не отвечает", attemptCount);
            }
            catch (Exception ex)
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

        await DisplayError($"{message}. Пробуем ещё раз...");
    }

    private Task DisplayFinalError()
    {
        return DisplayError("Не удаётся подключиться к серверу. Проверьте интернет и перезапустите приложение.");
    }

    private async Task DisplayError(string message)
    {
        await MainThread.InvokeOnMainThreadAsync(() =>
        {
            ErrorMessageLabel.Text = message;
            ErrorMessageLabel.IsVisible = true;
        }).ConfigureAwait(false);
    }

    private Task NavigateToMainPageAsync(bool isAuthorized)
    {
        ErrorMessageLabel.IsVisible = false;

        return isAuthorized
            ? _navigator.ChangePageTo<AuthorizedPage>(CancellationToken.None)
            : _navigator.ChangePageTo<LoginPage>(CancellationToken.None);
    }
}
