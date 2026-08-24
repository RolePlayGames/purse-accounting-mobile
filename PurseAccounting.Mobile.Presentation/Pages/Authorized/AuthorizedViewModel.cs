using PurseAccounting.Mobile.Application.Authorization;
using PurseAccountinng.Mobile.Presentation.Pages.Unauthorized.Login;
using PurseAccountinng.Mobile.Presentation.Services.MidnightDistributionManagers;
using PurseAccountinng.Mobile.Presentation.Services.Navigation;
using PurseAccountinng.Mobile.Presentation.Services.Notifications;
using PurseAccountinng.Mobile.Presentation.Services.Tabs;
using ReactiveUI;
using System.Windows.Input;

namespace PurseAccountinng.Mobile.Presentation.Pages.Authorized;

public class AuthorizedViewModel : ReactiveObject
{
    private readonly IAuthorizationService _authorizationService;
    private readonly INavigator _navigator;
    private readonly INotificationService _notificationService;
    private readonly ITabNavigator _tabNavigator;
    private readonly IMidnightDistributionManager _midnightDistributionManager;

    private AuthorizedTabBase? _activeTab;

    public AuthorizedTabBase? ActiveTab
    {
        get => _activeTab;
        set => this.RaiseAndSetIfChanged(ref _activeTab, value, nameof(ActiveTab));
    }

    public ICommand LogoutCommand { get; }

    public AuthorizedViewModel(
        IAuthorizationService authorizationService,
        INavigator navigator,
        INotificationService notificationService,
        ITabNavigator tabNavigator,
        IMidnightDistributionManager midnightDistributionManager)
    {
        _authorizationService = authorizationService;
        _navigator = navigator;
        _notificationService = notificationService;
        _tabNavigator = tabNavigator;
        _midnightDistributionManager = midnightDistributionManager;

        LogoutCommand = new Command(OnLogout);

        _tabNavigator.ActiveTabChanged += OnActiveTabChanged;

        OnActiveTabChanged(null, tabNavigator.ActiveTab);
    }

    public async void OnLogout()
    {
        var isSucceed = await _authorizationService.Logout(CancellationToken.None);

        if (isSucceed)
        {
            await _navigator.ChangePageTo<LoginPage>(CancellationToken.None);
            await _midnightDistributionManager.Stop();
        }
        else
        {
            _notificationService.ShowError("Что-то пошло не так при выходе. Повторите позже");
        }
    }

    public void ChangeTabTo<TTab>() where TTab : ContentView
    {
        _tabNavigator.ChangeTabTo<TTab>();
    }

    private void OnActiveTabChanged(AuthorizedTabBase? oldValue, AuthorizedTabBase? newValue)
    {
        if (newValue is null)
            return;

        ActiveTab = newValue;
    }
}
