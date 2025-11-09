namespace PurseAccountinng.Mobile.Presentation.Pages.Unauthorized.Login;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();

        var services = Application.Current?.Handler.MauiContext?.Services ?? throw new InvalidOperationException($"Cannot resolve service provider");
        var viewModel = ActivatorUtilities.CreateInstance<LoginViewModel>(services);

        BindingContext = viewModel;
    }

    private async void OnForgotPasswordTapped(object sender, EventArgs e)
    {
        await DisplayAlert("Восстановление", "Страница 'Забыли пароль?' пока не готова", "OK");
    }

    private async void OnRegisterTapped(object sender, EventArgs e)
    {
        await DisplayAlert("Регистрация", "Страница регистрации пока не готова", "OK");
    }

    private async void OnPrivacyPolicyTapped(object sender, EventArgs e)
    {
        await DisplayAlert("Политика конфиденциальности", "Страница политики конфиденциальности пока не готова", "OK");
    }
}
