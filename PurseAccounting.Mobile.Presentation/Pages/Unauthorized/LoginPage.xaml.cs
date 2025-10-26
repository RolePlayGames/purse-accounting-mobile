using PurseAccountinng.Mobile.Presentation.ViewModels.Unauthorized;
using ReactiveUI;
using System.Reactive.Linq;

namespace PurseAccountinng.Mobile.Presentation.Pages.Unauthorized;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();

        var services = Application.Current?.Handler.MauiContext?.Services;
        var viewModel = ActivatorUtilities.CreateInstance<LoginViewModel>(services);

        viewModel.WhenAnyValue(x => x.CanLogin)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(UpdateButtonAppearance);
    }

    private void OnTogglePasswordClicked(object sender, EventArgs e)
    {
        PasswordEntry.IsPassword = !PasswordEntry.IsPassword;

        TogglePasswordButton.Source = PasswordEntry.IsPassword
            ? "visibility_outlined.svg"
            : "visibility_off_outlined.svg";
    }

    private async void OnForgotPasswordTapped(object sender, EventArgs e)
    {
        // Позже: перейти на страницу восстановления
        await DisplayAlert("Восстановление", "Страница 'Забыли пароль?' пока не готова", "OK");
    }

    private async void OnRegisterTapped(object sender, EventArgs e)
    {
        // Позже: перейти на страницу регистрации
        await DisplayAlert("Регистрация", "Страница регистрации пока не готова", "OK");
    }

    private void UpdateButtonAppearance(bool canLogin)
    {
        if (canLogin)
        {
            LoginButton.BackgroundColor = Color.FromArgb("5A5386");
            LoginButton.TextColor = Color.FromArgb("FFFFFF");
        }
        else
        {
            LoginButton.BackgroundColor = Color.FromArgb("E0E0E0"); // InactiveElementFill
            LoginButton.TextColor = Color.FromArgb("8D8D8D");      // InactiveElementText
        }
    }

    private void OnLogginButtonPressed(object sender, EventArgs e)
    {
        LoginButton.BackgroundColor = Color.FromArgb("4C99CF");
    }

    private void OnLogginButtonReleased(object sender, EventArgs e)
    {
        LoginButton.BackgroundColor = Color.FromArgb("5A5386");
    }

    private void OnGoogleLoginTapped(object sender, EventArgs e)
    {
        // Визуальная обратная связь
        GoogleLogin.BackgroundColor = Color.FromArgb("E0E0E0");

        // Небольшая задержка для эффекта
        Task.Run(async () =>
        {
            await Task.Delay(150);
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                GoogleLogin.BackgroundColor = Colors.White;
                // Здесь будет логика входа через Google
                _ = DisplayAlert("Google", "Вход через Google пока не реализован", "OK");
            });
        });
    }

    private async void OnPrivacyPolicyTapped(object sender, EventArgs e)
    {
        // Позже: перейти на страницу политики конфиденциальности
        await DisplayAlert("Политика конфиденциальности", "Страница политики конфиденциальности пока не готова", "OK");
    }
}
