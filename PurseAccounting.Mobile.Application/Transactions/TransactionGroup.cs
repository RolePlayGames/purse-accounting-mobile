using PurseAccounting.Mobile.Application.Models;
using PurseAccounting.Mobile.Infrastructure.Transactions;

namespace PurseAccounting.Mobile.Application.Transactions;

public record TransactionGroup
{
    public required DateTime GroupDate { get; init; }

    public required IReadOnlyCollection<TransactionInfo> Transactions { get; init; }
}
