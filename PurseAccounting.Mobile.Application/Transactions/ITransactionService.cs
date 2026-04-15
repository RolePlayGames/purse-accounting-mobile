using PurseAccounting.Mobile.Infrastructure.Transactions;

namespace PurseAccounting.Mobile.Application.Transactions;

public interface ITransactionService
{
    Task<MakeTransactionResult> MakeTransaction(Transaction transaction, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<TransactionGroup>> GetTransactionsByDate(IReadOnlyCollection<long> categoryIds, short timeZone, CancellationToken cancellationToken);
}
