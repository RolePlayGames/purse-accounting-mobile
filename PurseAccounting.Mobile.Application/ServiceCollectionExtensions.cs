using Microsoft.Extensions.DependencyInjection;
using PurseAccounting.Mobile.Application.Accounting;
using PurseAccounting.Mobile.Application.Login;
using PurseAccounting.Mobile.Infrastructure;

namespace PurseAccounting.Mobile.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            return services
                .AddInfrastructure()
                .AddScoped<IAccountingService, AccountingService>()
                .AddScoped<ILoginService, LoginService>()
                ;
        }
    }
}
