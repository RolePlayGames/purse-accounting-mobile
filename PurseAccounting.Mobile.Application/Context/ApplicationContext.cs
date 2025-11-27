namespace PurseAccounting.Mobile.Application.Context;

internal class ApplicationContext : IApplicationContext
{
    private Account? _account;

    public Account? Account
    {
        get
        {
            return _account;
        }

        set
        {
            var oldValue = _account;

            if (oldValue == value)
                return;

            _account = value;
            AccountChanged?.Invoke(oldValue, value);
        }
    }

    public event AccountChangedEventHandler? AccountChanged;
}
