using PurseAccounting.Mobile.Application.Models;
using PurseAccounting.Mobile.Infrastructure.Transactions;

namespace PurseAccounting.Mobile.Application.Transactions;

public interface ITransactionService
{
    Task<MakeTransactionResult> MakeTransaction(Transaction transaction, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<IGrouping<DateTime, DateWithTimeZone>>> GetTransactionsByDate(
        IReadOnlyCollection<long> categoryIds,
        short timeZone,
        CancellationToken cancellationToken);
}
