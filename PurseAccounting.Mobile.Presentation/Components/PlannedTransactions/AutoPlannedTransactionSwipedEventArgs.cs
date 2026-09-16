using PurseAccounting.Mobile.Infrastructure.PlannedTransactions.Settings;

namespace PurseAccountinng.Mobile.Presentation.Components.PlannedTransactions;

public class AutoPlannedTransactionSwipedEventArgs : EventArgs
{
    public PlannedTransactionSettingInfo PlannedTransactionSettingInfo { get; }

    public AutoPlannedTransactionSwipedEventArgs(PlannedTransactionSettingInfo plannedTransactionSettingInfo)
    {
        PlannedTransactionSettingInfo = plannedTransactionSettingInfo;
    }
}
