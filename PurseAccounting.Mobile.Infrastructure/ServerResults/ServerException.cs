namespace PurseAccounting.Mobile.Infrastructure.ServerResults;

public class ServerException<T> : Exception
{
    public required T NoticeType { get; init; }
}
