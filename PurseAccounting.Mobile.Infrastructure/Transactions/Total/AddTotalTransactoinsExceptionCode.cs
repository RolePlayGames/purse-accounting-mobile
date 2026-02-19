using System.Text.Json.Serialization;

namespace PurseAccounting.Mobile.Infrastructure.Transactions.Total;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AddTotalTransactoinsExceptionCode
{
    PlannedDateHasPassed,
    NegativeRestAmount,
}
