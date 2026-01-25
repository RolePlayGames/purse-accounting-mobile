using PurseAccounting.Mobile.Application.Models;
using PurseAccounting.Mobile.Infrastructure.Accounting.TransactionCategories;

namespace PurseAccounting.Mobile.Application.Context;

public delegate void ValueChangedEventHandler<T>(T? oldValue, T? newValue) where T : class;

public interface IApplicationContext
{
    /// <summary>
    /// Active account
    /// </summary>
    Account? Account { get; set; }

    /// <summary>
    /// User's transaction categories
    /// </summary>
    IReadOnlyCollection<TransactionCategoryDto> TransactionCategories { get; set; }

    /// <summary>
    /// On account changed
    /// </summary>
    event ValueChangedEventHandler<Account> AccountChanged;

    event ValueChangedEventHandler<IReadOnlyCollection<TransactionCategoryDto>> TransactionCategoriesChanged;
}
