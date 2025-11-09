namespace PurseAccounting.Mobile.Application.Login
{
    public interface ILoginService
    {
        Task Login(string login, string password, CancellationToken cancellationToken);
    }
}
