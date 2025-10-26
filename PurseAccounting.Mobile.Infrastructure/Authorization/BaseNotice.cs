namespace PurseAccounting.Mobile.Infrastructure.Authorization
{
    public record BaseNotice
    {
        public required string NoticeType { get; init; }
    }
}
