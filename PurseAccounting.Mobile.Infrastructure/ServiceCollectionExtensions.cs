using Microsoft.Extensions.DependencyInjection;
using PurseAccounting.Mobile.Infrastructure.Authorization.MailboxAuthorization;

namespace PurseAccounting.Mobile.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            var httpClient = new HttpClient { BaseAddress = new Uri("https://purse-accounting.ru") };

            services
                .AddSingleton(httpClient)
                .AddScoped<IMailboxAuthorizationClient, MailboxAuthorizationClient>();

            return services;
        }
    }
}
