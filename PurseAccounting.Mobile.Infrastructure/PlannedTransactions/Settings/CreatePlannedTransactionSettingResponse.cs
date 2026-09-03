namespace PurseAccounting.Mobile.Infrastructure.PlannedTransactions.Settings;

/// <summary>
/// Response after creating a planned transaction setting
/// </summary>
public record CreatePlannedTransactionSettingResponse
{
    public required long PlannedTransactionSettingID { get; init; }

    public required Transactions.AccountAmounts AccountAmounts { get; init; }
}
