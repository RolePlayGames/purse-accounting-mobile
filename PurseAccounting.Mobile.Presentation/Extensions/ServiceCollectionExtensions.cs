using Microsoft.Extensions.DependencyInjection;
using PurseAccounting.Mobile.Application;
using PurseAccountinng.Mobile.Presentation.Pages;
using PurseAccountinng.Mobile.Presentation.Pages.Authorized;
using PurseAccountinng.Mobile.Presentation.Pages.Authorized.Account;
using PurseAccountinng.Mobile.Presentation.Pages.Authorized.Accounting;
using PurseAccountinng.Mobile.Presentation.Pages.Authorized.Distribution;
using PurseAccountinng.Mobile.Presentation.Pages.Authorized.Transactions;
using PurseAccountinng.Mobile.Presentation.Pages.Unauthorized.Login;
using PurseAccountinng.Mobile.Presentation.Services.MidnightDistributionManagers;
using PurseAccountinng.Mobile.Presentation.Services.Navigation;
using PurseAccountinng.Mobile.Presentation.Services.Notifications;
using PurseAccountinng.Mobile.Presentation.Services.Tabs;

namespace PurseAccounting.Mobile.Presentation;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPages(this IServiceCollection services)
    {
        return services
            .AddSingleton<AuthorizedPage>()
            .AddSingleton<LoginPage>()
            .AddSingleton<LogoPage>()
            ;
    }

    public static IServiceCollection AddTabs(this IServiceCollection services)
    {
        return services
            .AddSingleton<AccountingTab>()
            .AddSingleton<DistributionTab>()
            .AddSingleton<TransactionsTab>()
            .AddSingleton<AccountTab>()
            .AddSingleton<UserProfileTab>()
            .AddSingleton<CategoriesTab>()
            ;
    }

    public static IServiceCollection AddViewModels(this IServiceCollection services)
    {
        return services
            .AddSingleton<AccountAttributesWidgetViewModel>()
            .AddSingleton<AccountWidgetViewModel>()
            .AddSingleton<TransactionAttributesViewModel>()
            .AddSingleton<TomorrowAmountWidgetViewModel>()
            .AddSingleton<TransactionsTabViewModel>()
            .AddSingleton<AuthorizedViewModel>()
            .AddSingleton<DistributionTabViewModel>()
            ;
    }

    public static IServiceCollection AddPresentationServices(this IServiceCollection services)
    {
        return services
            .AddSingleton<IMidnightDistributionManager, MidnightDistributionManager>()
            .AddSingleton<INavigator, Navigator>()
            .AddSingleton<INotificationService, NotificationService>()
            .AddSingleton<ITabNavigator, TabNavigator>()
            ;
    }
}
