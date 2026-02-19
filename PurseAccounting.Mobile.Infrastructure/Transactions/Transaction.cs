namespace PurseAccounting.Mobile.Infrastructure.Transactions;

public class Transaction
{
    public int Amount { get; set; }

    public long TransactionCategoryID { get; set; }

    public TransactionChangeAmountType ChangeAmountType { get; set; }

    public TransactionChangeType ChangeType { get; set; }

    public DateTime TransactionDate { get; set; }
}
