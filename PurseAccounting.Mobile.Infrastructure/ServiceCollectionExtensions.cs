using Microsoft.Extensions.DependencyInjection;
using PurseAccounting.Mobile.Infrastructure.Accounting;
using PurseAccounting.Mobile.Infrastructure.Accounting.TransactionCategories;
using PurseAccounting.Mobile.Infrastructure.AuthCookieStorages;
using PurseAccounting.Mobile.Infrastructure.Authorization.MailboxAuthorization;
using PurseAccounting.Mobile.Infrastructure.HttpClientInitializers;
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
                .AddScoped<IDailyTransactionClient, DailyTransactionClient>()
                .AddScoped<IMailboxAuthorizationClient, MailboxAuthorizationClient>()
                .AddScoped<ITotalTransactionClient, TotalTransactionClient>()
                .AddScoped<ITransactionCategoryClient, TransactionCategoryClient>()
                .AddTransient<IAuthCookieStorage, AuthCookieStorage>()
                .AddTransient<IHttpClientInitializer, HttpClientInitializer>()
                ;
        }
    }
}
