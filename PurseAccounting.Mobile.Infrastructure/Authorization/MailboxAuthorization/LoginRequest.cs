namespace PurseAccounting.Mobile.Infrastructure.Authorization.MailboxAuthorization
{
    internal record LoginRequest
    {
        public required string Login { get; init; }

        public required string Password { get; init; }
    }
}
