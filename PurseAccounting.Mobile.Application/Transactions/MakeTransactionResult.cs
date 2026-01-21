namespace PurseAccounting.Mobile.Application.Transactions;

public enum MakeTransactionResult
{
    Success,
    PlannedDateHasPassed,
    NegativeRestAmount,
    Unknown,
}
