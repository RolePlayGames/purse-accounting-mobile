using Microsoft.Extensions.DependencyInjection;
using PurseAccounting.Mobile.Application.AccountFactories;
using PurseAccounting.Mobile.Application.Accounts;
using PurseAccounting.Mobile.Application.Authorization;
using PurseAccounting.Mobile.Application.Calculators.AmountsCalculators;
using PurseAccounting.Mobile.Application.Calculators.AmountsDistributionCalculators;
using PurseAccounting.Mobile.Application.Calculators.DaysCountCalculators;
using PurseAccounting.Mobile.Application.Calculators.TomorrowAmountCalculators;
using PurseAccounting.Mobile.Application.Context;
using PurseAccounting.Mobile.Application.Distribution;
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
                .AddScoped<IAccountService, AccountService>()
                .AddScoped<IAccountFactory, AccountFactory>()
                .AddScoped<IAmountsCalculator, AmountsCalculator>()
                .AddScoped<IAmountsDistributionCalculator, AmountsDistributionCalculator>()
                .AddScoped<IAuthorizationService, AuthorizationService>()
                .AddScoped<IDaysCountCalculators, DaysCountCalculators>()
                .AddScoped<ITomorrowAmountCalculator, TomorrowAmountCalculator>()
                .AddScoped<ITransactionCategoriesService, TransactionCategoriesService>()
                .AddScoped<ITransactionService, TransactionService>()
                .AddScoped<IDistributionService, DistributionService>()
                .AddSingleton<IApplicationContext, ApplicationContext>()
                ;
        }
    }
}
