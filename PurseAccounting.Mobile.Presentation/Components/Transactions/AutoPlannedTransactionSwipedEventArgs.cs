using PurseAccounting.Mobile.Infrastructure.PlannedTransactions.Settings;

namespace PurseAccountinng.Mobile.Presentation.Components.Transactions;

public class AutoPlannedTransactionSwipedEventArgs : EventArgs
{
    public PlannedTransactionSettingInfo PlannedTransactionSettingInfo { get; }

    public AutoPlannedTransactionSwipedEventArgs(PlannedTransactionSettingInfo plannedTransactionSettingInfo)
    {
        PlannedTransactionSettingInfo = plannedTransactionSettingInfo;
    }
}
