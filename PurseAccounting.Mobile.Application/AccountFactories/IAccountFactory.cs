using PurseAccounting.Mobile.Application.Context;

namespace PurseAccounting.Mobile.Application.AccountFactories;

public interface IAccountFactory
{
    /// <summary>
    /// Creates account model from account dto
    /// </summary>
    /// <param name="account">Account dto</param>
    /// <returns>Account model</returns>
    Account CreateAccount(Infrastructure.Accounting.Account account);
}
