using Microsoft.Extensions.DependencyInjection;
using PurseAccounting.Mobile.Application.AccountFactories;
using PurseAccounting.Mobile.Application.Accounting;
using PurseAccounting.Mobile.Application.Calculators.AmountsCalculators;
using PurseAccounting.Mobile.Application.Calculators.AmountsDistributionCalculators;
using PurseAccounting.Mobile.Application.Calculators.TomorrowAmountCalculators;
using PurseAccounting.Mobile.Application.Context;
using PurseAccounting.Mobile.Application.Login;
using PurseAccounting.Mobile.Application.TransactionCategories;
using PurseAccounting.Mobile.Application.Transactions;
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
                .AddScoped<IAccountFactory, AccountFactory>()
                .AddScoped<IAmountsCalculator, AmountsCalculator>()
                .AddScoped<IAmountsDistributionCalculator, AmountsDistributionCalculator>()
                .AddScoped<ILoginService, LoginService>()
                .AddScoped<ITomorrowAmountCalculator, TomorrowAmountCalculator>()
                .AddScoped<ITransactionCategoriesService, TransactionCategoriesService>()
                .AddScoped<ITransactionService, TransactionService>()
                .AddSingleton<IApplicationContext, ApplicationContext>()
                ;
        }
    }
}
