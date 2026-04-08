namespace PurseAccounting.Mobile.Application.Models;

public record DateWithTimeZone
{
    public required DateTime Value { get; init; }

    public required short TimeZone { get; init; }

    public required long TransactionId { get; init; }

    public DateTime Date => Value.Date;

    public DateTime ValueWithTimeZone => Value.AddHours(TimeZone);

    public static explicit operator DateTime(DateWithTimeZone date) => date.Value;

    public static implicit operator DateWithTimeZone(DateTime dateTime) => new() { Value = dateTime, TimeZone = 0, TransactionId = 0 };
}
