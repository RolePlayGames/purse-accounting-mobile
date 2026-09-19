using PurseAccounting.Mobile.Infrastructure.PlannedTransactions.Settings.Periods;

namespace PurseAccounting.Mobile.Infrastructure.PlannedTransactions.Settings;

/// <summary>
/// Planned transaction setting information
/// </summary>
public record PlannedTransactionSettingInfo
{
    public required long ID { get; init; }

    public required string Name { get; init; }

    public required int Amount { get; init; }

    public required long TransactionCategoryID { get; init; }

    public required PeriodInfo Period { get; init; }

    public required Transactions.TransactionChangeType ChangeType { get; init; }

    public required bool IsAutomatic { get; init; }
}
