namespace PurseAccounting.Mobile.Infrastructure.Authorization.MailboxAuthorization
{
    public interface IMailboxAuthorizationClient
    {
        Task<MailboxAuthorizationEnum> Login(string email, string password, CancellationToken cancellationToken);
    }
}
