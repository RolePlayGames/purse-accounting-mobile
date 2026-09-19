namespace PurseAccounting.Mobile.Infrastructure.PlannedTransactions.Awaiting;

public enum MakeAwaitingPlannedTransactionExceptionCode
{
    AwaitingPlannedTransactionWasNotFound,
    PlannedTransactionSettingWasNotFound,
    FirstAwaitingPlannedTransactionIsAlreadyExists,
}
