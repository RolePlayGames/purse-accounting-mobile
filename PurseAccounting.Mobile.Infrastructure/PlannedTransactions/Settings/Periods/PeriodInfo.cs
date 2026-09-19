using System.Text.Json.Serialization;

namespace PurseAccounting.Mobile.Infrastructure.PlannedTransactions.Settings.Periods;

/// <summary>
/// Base period information for planned transaction settings
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(DailyPeriodInfo), typeDiscriminator: "daily")]
[JsonDerivedType(typeof(MonthlyPeriodInfo), typeDiscriminator: "monthly")]
[JsonDerivedType(typeof(AnnuallyPeriodInfo), typeDiscriminator: "annually")]
[JsonDerivedType(typeof(OncePeriodInfo), typeDiscriminator: "once")]
[JsonDerivedType(typeof(WeeklyPeriodInfo), typeDiscriminator: "weekly")]
public abstract record PeriodInfo;
