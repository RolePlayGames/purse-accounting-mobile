using PurseAccounting.Mobile.Application;
using PurseAccountinng.Mobile.Presentation.Pages;
using PurseAccountinng.Mobile.Presentation.Pages.Authorized;
using PurseAccountinng.Mobile.Presentation.Pages.Unauthorized.Login;
using PurseAccountinng.Mobile.Presentation.Services.Navigation;
using PurseAccountinng.Mobile.Presentation.Services.Notifications;

namespace PurseAccounting.Mobile.Presentation;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPages(this IServiceCollection services)
    {
        return services
            .AddApplication()
            .AddTransient<AuthorizedPage>()
            .AddTransient<LoginPage>()
            .AddTransient<LogoPage>()
            ;
    }

    public static IServiceCollection AddPresentationServices(this IServiceCollection services)
    {
        return services
            .AddSingleton<INavigator, Navigator>()
            .AddSingleton<INotificationService, NotificationService>()
            ;
    }
}
