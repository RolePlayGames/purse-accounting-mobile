using PurseAccounting.Mobile.Application.Models;

namespace PurseAccounting.Mobile.Application.Context;

public delegate void AccountChangedEventHandler(Account? oldValue, Account? newValue);

public interface IApplicationContext
{
    Account? Account { get; set; }

    event AccountChangedEventHandler AccountChanged;
}
