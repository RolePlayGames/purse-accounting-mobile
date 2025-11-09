namespace PurseAccounting.Mobile.Infrastructure.Authorization.MailboxAuthorization
{
    public interface IMailboxAuthorizationClient
    {
        Task<MailboxAuthorizationEnum> Login(string login, string password, CancellationToken cancellationToken);
    }
}
