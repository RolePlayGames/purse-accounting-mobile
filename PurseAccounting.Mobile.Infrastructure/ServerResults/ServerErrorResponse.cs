namespace PurseAccounting.Mobile.Infrastructure.ServerResults;

public record ServerErrorResponse<T>
{
    public required T NoticeType { get; init; }
}
