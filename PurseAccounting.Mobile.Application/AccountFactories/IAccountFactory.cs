using PurseAccounting.Mobile.Application.Models;
using PurseAccounting.Mobile.Infrastructure.Accounting;

namespace PurseAccounting.Mobile.Application.AccountFactories;

public interface IAccountFactory
{
    /// <summary>
    /// Creates account from dto
    /// </summary>
    /// <param name="account">Account dto</param>
    /// <returns>Account</returns>
    Account GetAccount(AccountDto account);

    /// <summary>
    /// Creates new account with updated amount
    /// </summary>
    /// <param name="account">Account</param>
    /// <param name="amount">Distributed amound</param>
    /// <returns>Updated account</returns>
    Account GetAccount(Account account, DailyDistributedAmount amount);
}
