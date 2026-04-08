namespace PurseAccounting.Mobile.Infrastructure.Transactions;

public record struct TransactionInfo(
    long ID,
    int Amount,
    DateTime Date,
    string ChangeAmountType,
    long TransactionCategoryID)
{
}
