namespace PurseAccounting.Mobile.Infrastructure.Transactions;

/// <summary>
/// Account state after transaction was made
/// </summary>
public record AddTransactionResponse(long RestAmount, long DayAmount);
