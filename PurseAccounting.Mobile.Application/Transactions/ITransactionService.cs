using PurseAccounting.Mobile.Infrastructure.Transactions;

namespace PurseAccounting.Mobile.Application.Transactions;

public interface ITransactionService
{
    Task<bool> MakeTransaction(Transaction transaction, CancellationToken cancellationToken);
}
