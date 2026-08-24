using PurseAccounting.Mobile.Application.Distribution;
using PurseAccounting.Mobile.Application.Models;
using PurseAccounting.Mobile.Infrastructure.TransactionCategories;

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
    /// Avaliable distribution strategy
    /// </summary>
    AvailableUserChoiceDistributionStrategyInfo? AvailableUserChoiceDistributionStrategy { get; set; }

    /// <summary>
    /// On account changed
    /// </summary>
    event ValueChangedEventHandler<Account> AccountChanged;

    /// <summary>
    /// On transactions changed
    /// </summary>
    event ValueChangedEventHandler<IReadOnlyCollection<TransactionCategoryDto>> TransactionCategoriesChanged;

    /// <summary>
    /// On distribution strategy changed
    /// </summary>
    event ValueChangedEventHandler<AvailableUserChoiceDistributionStrategyInfo> AvailableUserChoiceDistributionStrategyChanged;
}
