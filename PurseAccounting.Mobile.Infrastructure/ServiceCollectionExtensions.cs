using Microsoft.Extensions.DependencyInjection;
using PurseAccounting.Mobile.Infrastructure.Accounts;
using PurseAccounting.Mobile.Infrastructure.AuthCookieStorages;
using PurseAccounting.Mobile.Infrastructure.Authorization;
using PurseAccounting.Mobile.Infrastructure.Authorization.MailboxAuthorization;
using PurseAccounting.Mobile.Infrastructure.Distribution;
using PurseAccounting.Mobile.Infrastructure.HttpClientInitializers;
using PurseAccounting.Mobile.Infrastructure.PlannedTransactions.Awaiting;
using PurseAccounting.Mobile.Infrastructure.PlannedTransactions.Settings;
using PurseAccounting.Mobile.Infrastructure.TransactionCategories;
using PurseAccounting.Mobile.Infrastructure.Transactions;
using PurseAccounting.Mobile.Infrastructure.Transactions.Daily;
using PurseAccounting.Mobile.Infrastructure.Transactions.Total;

namespace PurseAccounting.Mobile.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            var httpClient = new HttpClient { BaseAddress = new Uri("https://purse-accounting.ru") };

            return services
                .AddSingleton(httpClient)
                .AddScoped<IAccountClient, AccountClient>()
                .AddScoped<IAuthorizationClient, AuthorizationClient>()
                .AddScoped<IAwaitingPlannedTransactionClient, AwaitingPlannedTransactionClient>()
                .AddScoped<IPlannedTransactionSettingsClient, PlannedTransactionSettingsClient>()
                .AddScoped<IDailyTransactionClient, DailyTransactionClient>()
                .AddScoped<IDistributionClient, DistributionClient>()
                .AddScoped<IMailboxAuthorizationClient, MailboxAuthorizationClient>()
                .AddScoped<ITotalTransactionClient, TotalTransactionClient>()
                .AddScoped<ITransactionCategoryClient, TransactionCategoryClient>()
                .AddScoped<ITransactionsClient, TransactionsClient>()
                .AddTransient<IAuthCookieStorage, AuthCookieStorage>()
                .AddTransient<IHttpClientInitializer, HttpClientInitializer>()
                ;
        }
    }
}
