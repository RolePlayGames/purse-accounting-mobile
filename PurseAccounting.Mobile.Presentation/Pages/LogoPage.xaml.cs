using PurseAccounting.Mobile.Application.Accounts;
using PurseAccounting.Mobile.Infrastructure.HttpClientInitializers;
using PurseAccountinng.Mobile.Presentation.Pages.Unauthorized.Login;
using PurseAccountinng.Mobile.Presentation.Services.Navigation;

namespace PurseAccountinng.Mobile.Presentation.Pages;

public partial class LogoPage : ContentPage
{
    private const int _maxAttempts = 20;
    private static readonly TimeSpan _requestTimeout = TimeSpan.FromSeconds(2);

    private readonly IHttpClientInitializer _httpClientInitializer;
    private readonly IAccountService _accountingService;
    private readonly INavigator _navigator;

    public LogoPage(IHttpClientInitializer httpClientInitializer, IAccountService accountingService, INavigator navigator)
    {
        _httpClientInitializer = httpClientInitializer;
        _accountingService = accountingService;
        _navigator = navigator;

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

                await NavigateToMainPage(account is not null);

                return;
            }
            catch (TaskCanceledException)
            {
                await HandleError("Сервер долго не отвечает", attemptCount);
            }
            catch (Exception ex)
            {
#if DEBUG
                await HandleError($"Ошибка подключения: {ex}", attemptCount);
#else
                await HandleError($"Ошибка подключения", attemptCount);
#endif
            }

            await Task.Delay(100);
        }

        await DisplayFinalError();
        await Task.Delay(_requestTimeout);
        await NavigateToMainPage(false);
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

    private async Task NavigateToMainPage(bool isAuthorized)
    {
        ErrorMessageLabel.IsVisible = false;

        if (isAuthorized)
            await _navigator.ActivateAuthorizedPage(CancellationToken.None);
        else
            await _navigator.ChangePageTo<LoginPage>(CancellationToken.None);
    }
}
