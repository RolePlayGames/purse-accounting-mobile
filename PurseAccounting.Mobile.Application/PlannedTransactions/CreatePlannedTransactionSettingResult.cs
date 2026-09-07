namespace PurseAccounting.Mobile.Application.PlannedTransactions;

public enum CreatePlannedTransactionSettingResult
{
    Success,
    FirstAwaitingPlannedTransactionIsAlreadyExists,
    PlannedTransactionSettingsNameAlreadyExists,
    Unknown,
}
