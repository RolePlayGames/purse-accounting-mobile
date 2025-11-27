using PurseAccounting.Mobile.Application;
using PurseAccountinng.Mobile.Presentation.Pages;
using PurseAccountinng.Mobile.Presentation.Pages.Authorized;
using PurseAccountinng.Mobile.Presentation.Pages.Unauthorized.Login;

namespace PurseAccounting.Mobile.Presentation
{
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
    }
}
