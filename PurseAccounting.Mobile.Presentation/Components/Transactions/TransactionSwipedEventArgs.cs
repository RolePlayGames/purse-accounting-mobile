using PurseAccounting.Mobile.Infrastructure.Transactions;

namespace PurseAccountinng.Mobile.Presentation.Components.Transactions;

public class TransactionSwipedEventArgs : EventArgs
{
    public TransactionInfo Transaction { get; }

    public TransactionSwipedEventArgs(TransactionInfo transaction)
    {
        Transaction = transaction;
    }
}
