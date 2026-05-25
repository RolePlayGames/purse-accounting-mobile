using System.Text.Json.Serialization;

namespace PurseAccounting.Mobile.Infrastructure.Distribution;

/// <summary>
/// Information about distribution strategy
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "Type")]
[JsonDerivedType(typeof(DistributionStrategyInfo), "Automatic")]
[JsonDerivedType(typeof(DistributionStrategyInfo), "DoNotNeed")]
[JsonDerivedType(typeof(UserChoiceDistributionStrategyInfo), "UserChoice")]
public record DistributionStrategyInfo
{
    public required string Type { get; init; }
}
