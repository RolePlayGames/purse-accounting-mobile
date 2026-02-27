using PurseAccounting.Mobile.Application.Models;
using PurseAccounting.Mobile.Infrastructure.TransactionCategories;

namespace PurseAccounting.Mobile.Application.Context;

internal class ApplicationContext : IApplicationContext
{
    private Account? _account;
    private IReadOnlyCollection<TransactionCategoryDto> _transactionCategories = [];

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

    public IReadOnlyCollection<TransactionCategoryDto> TransactionCategories
    {
        get
        {
            return _transactionCategories;
        }

        set
        {
            var oldValue = _transactionCategories;

            if (oldValue.Count == value.Count && oldValue.Except(value).Any())
                return;

            _transactionCategories = value;
            TransactionCategoriesChanged?.Invoke(oldValue, value);
        }
    }

    public event ValueChangedEventHandler<Account>? AccountChanged;

    public event ValueChangedEventHandler<IReadOnlyCollection<TransactionCategoryDto>>? TransactionCategoriesChanged;
}
