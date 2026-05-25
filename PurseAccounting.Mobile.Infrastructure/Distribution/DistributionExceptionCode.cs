using System.Text.Json.Serialization;

namespace PurseAccounting.Mobile.Infrastructure.Distribution;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DistributionExceptionCode
{
    DistributionIsNotNeeded,
}
