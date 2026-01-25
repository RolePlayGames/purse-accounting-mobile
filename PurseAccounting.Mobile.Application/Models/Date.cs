namespace PurseAccounting.Mobile.Application.Models;

public record Date
{
    private DateTime _value;

    public required DateTime Value { get => _value; init => _value = value.Date; }

    public static explicit operator DateTime(Date date) => date.Value;

    public static implicit operator Date(DateTime dateTime) => new() { Value = dateTime };

    public static TimeSpan operator -(Date date1, Date date2) => date1.Value - date2.Value;

    public static bool operator >(Date date1, Date date2) => date1.Value > date2.Value;

    public static bool operator <(Date date1, Date date2) => date1.Value < date2.Value;

    public static bool operator >=(Date date1, Date date2) => date1.Value >= date2.Value;

    public static bool operator <=(Date date1, Date date2) => date1.Value <= date2.Value;

    public Date AddDays(int daysCount)
    {
        return new() { Value = Value.AddDays(daysCount) };
    }
}
