namespace PurseAccounting.Mobile.Infrastructure.Transactions;

/// <summary>
/// Account state after transaction make
/// </summary>
public record AddTransactionResponse(
    long RestAmount,
    long DayAmount)
{
}
