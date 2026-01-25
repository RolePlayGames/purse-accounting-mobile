namespace PurseAccounting.Mobile.Application.Context;

public delegate void AccountChangedEventHandler(Account? oldValue, Account? newValue);

public interface IApplicationContext
{
    /// <summary>
    /// Active account
    /// </summary>
    Account? Account { get; set; }

    /// <summary>
    /// On account changing
    /// </summary>
    event AccountChangedEventHandler AccountChanged;
}
