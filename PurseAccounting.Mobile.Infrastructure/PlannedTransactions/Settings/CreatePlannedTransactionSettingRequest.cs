using PurseAccounting.Mobile.Infrastructure.PlannedTransactions.Settings.Periods;

namespace PurseAccounting.Mobile.Infrastructure.PlannedTransactions.Settings;

/// <summary>
/// Request to create a planned transaction setting
/// </summary>
public record CreatePlannedTransactionSettingRequest
{
    public required string Name { get; init; }

    public required int Amount { get; init; }

    public required long TransactionCategoryID { get; init; }

    public required PeriodInfo Period { get; init; }

    public required Transactions.TransactionChangeType ChangeType { get; init; }

    public required bool IsAutomatic { get; init; }
}
