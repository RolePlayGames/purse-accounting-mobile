using PurseAccounting.Mobile.Application.Context;
using PurseAccounting.Mobile.Application.Distribution;
using PurseAccountinng.Mobile.Presentation.Pages.Authorized;
using PurseAccountinng.Mobile.Presentation.Pages.Authorized.Accounting;
using PurseAccountinng.Mobile.Presentation.Pages.Authorized.Distribution;
using PurseAccountinng.Mobile.Presentation.Services.MidnightDistributionManagers;
using PurseAccountinng.Mobile.Presentation.Services.Tabs;

namespace PurseAccountinng.Mobile.Presentation.Services.Navigation;

internal class Navigator : INavigator
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ITabNavigator _tabNavigator;
    private readonly IDistributionService _distributionService;
    private readonly IMidnightDistributionManager _midnightDistributionManager;
    private readonly IApplicationContext _applicationContext;

    public Navigator(
        IServiceProvider serviceProvider,
        ITabNavigator tabNavigator,
        IDistributionService distributionService,
        IMidnightDistributionManager midnightDistributionManager,
        IApplicationContext applicationContext)
    {
        _serviceProvider = serviceProvider;
        _tabNavigator = tabNavigator;
        _distributionService = distributionService;
        _midnightDistributionManager = midnightDistributionManager;
        _applicationContext = applicationContext;
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

    public async Task ChangeTabTo<TTab>(CancellationToken ct) where TTab : ContentView
    {
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            var page = Application.Current?.Windows[0].Page is NavigationPage navigationPage ? navigationPage.RootPage : Application.Current?.Windows[0].Page;

            if (Application.Current?.Windows[0].Page is not null)
            {
                if (page is not AuthorizedPage)
                {
                    page = _serviceProvider.GetRequiredService<AuthorizedPage>();
                    Application.Current.Windows[0].Page = new NavigationPage(page);
                }

                _tabNavigator.ChangeTabTo<TTab>();
            }
        }).ConfigureAwait(false);
    }

    public async Task ActivateAuthorizedPage(CancellationToken ct)
    {
        _tabNavigator.InitializeTabs();

        var distributionStrategy = await _distributionService.GetAvailableUserChoiceDistributionStrategy(ct);

        if (distributionStrategy is not null)
            await ChangeTabTo<DistributionTab>(ct);
        else
            await ChangeTabTo<AccountingTab>(ct);

        if (_applicationContext.Account is not null)
            await _midnightDistributionManager.Start(_applicationContext.Account.TimeZone, ct);
    }
}
