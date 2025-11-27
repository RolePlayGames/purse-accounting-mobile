using PurseAccounting.Mobile.Application.Context;

namespace PurseAccounting.Mobile.Application.AccountFactories;

public interface IAccountFactory
{
    Account GetAccount(Infrastructure.Accounting.Account account);
}
