using PurseAccounting.Mobile.Application.Models;

namespace PurseAccounting.Mobile.Application.AccountFactories;

public interface IAccountFactory
{
    Account GetAccount(Infrastructure.Accounting.Account account);
}
