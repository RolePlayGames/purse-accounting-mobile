using PurseAccounting.Mobile.Application.Authorization;
using PurseAccountinng.Mobile.Presentation.Pages.Unauthorized.Login;
using PurseAccountinng.Mobile.Presentation.Services.Navigation;
using PurseAccountinng.Mobile.Presentation.Services.Notifications;
using ReactiveUI;
using System.Windows.Input;

namespace PurseAccountinng.Mobile.Presentation.Pages.Authorized;

public class AuthorizedViewModel : ReactiveObject
{
    private readonly IAuthorizationService _authorizationService;
    private readonly INavigator _navigator;
    private readonly INotificationService _notificationService;

    private string? _header;
    private bool _isTabbarVisible = true;

    public string? Header
    {
        get => _header;
        set => this.RaiseAndSetIfChanged(ref _header, value, nameof(Header));
    }

    public bool IsTabbarVisible
    {
        get => _isTabbarVisible;
        set => this.RaiseAndSetIfChanged(ref _isTabbarVisible, value, nameof(IsTabbarVisible));
    }

    public ICommand LogoutCommand { get; }

    public AuthorizedViewModel(IAuthorizationService authorizationService, INavigator navigator, INotificationService notificationService)
    {
        _authorizationService = authorizationService;
        _navigator = navigator;
        _notificationService = notificationService;

        LogoutCommand = new Command(OnLogout);
    }

    public async void OnLogout()
    {
        var isSucceed = await _authorizationService.Logout(CancellationToken.None);

        if (isSucceed)
            await _navigator.ChangePageTo<LoginPage>(CancellationToken.None);
        else
            _notificationService.ShowError("Что-то пошло не так при выходе. Повторите позже");
    }
}
