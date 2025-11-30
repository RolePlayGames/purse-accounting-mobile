using PurseAccounting.Mobile.Application.Models;

namespace PurseAccounting.Mobile.Application.Accounting;

public interface IAccountingService
{
    /// <summary>
    /// Loads account
    /// </summary>
    /// <returns>Account or null</returns>
    Task<Account?> LoadAccount(CancellationToken cancellationToken);
}
