using PurseAccounting.Mobile.Application.Models;

namespace PurseAccounting.Mobile.Application.Accounts;

public interface IAccountService
{
    /// <summary>
    /// Loads account
    /// </summary>
    /// <returns>Account or null</returns>
    Task<Account?> LoadAccount(CancellationToken cancellationToken);

    /// <summary>
    /// Update account
    /// </summary>
    /// <param name="totalAmount">Account total amount</param>
    /// <param name="plannedDate">Account planned date</param>
    /// <param name="timeZone">Account time zone</param>
    /// <returns>Operation status</returns>
    Task<UpdateAccountResult> UpdateAccount(long totalAmount, DateTime plannedDate, short timeZone, CancellationToken cancellationToken);
}
