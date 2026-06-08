using PurseAccounting.Mobile.Application.Context;
using PurseAccountinng.Mobile.Presentation.Pages.Authorized;
using PurseAccountinng.Mobile.Presentation.Pages.Authorized.Account;
using PurseAccountinng.Mobile.Presentation.Pages.Authorized.Accounting;
using PurseAccountinng.Mobile.Presentation.Pages.Authorized.Distribution;
using PurseAccountinng.Mobile.Presentation.Pages.Authorized.Transactions;
using ReactiveUI;
using System.Collections.Concurrent;

namespace PurseAccountinng.Mobile.Presentation.Services.Tabs;

internal class TabNavigator : ITabNavigator
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ConcurrentDictionary<Type, AuthorizedTabBase> _tabsCollection = new();

    private AuthorizedTabBase? _defaultTab;
    private AuthorizedTabBase? _activeTab;

    public AuthorizedTabBase? ActiveTab
    {
        get
        {
            return _activeTab;
        }

        private set
        {
            var oldValue = _activeTab;

            if (oldValue != value)
            {
                _activeTab = value;
                ActiveTabChanged?.Invoke(oldValue, _activeTab);
            }
        }
    }

    public event ValueChangedEventHandler<AuthorizedTabBase>? ActiveTabChanged;

    public TabNavigator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void ChangeTabTo<TTab>() where TTab : ContentView
    {
        var tabType = typeof(TTab);
        var tab = _tabsCollection.GetValueOrDefault(tabType);

        ActiveTab = tab ?? _defaultTab;
    }

    public async Task InitializeTabs()
    {
        if (!_tabsCollection.IsEmpty)
            return;

        ActiveTab = _defaultTab = AddTab<AccountingTab>(_serviceProvider, "Добавить транзакцию");
        AddTab<DistributionTab>(_serviceProvider, "Распределение остатка", false);

        var tasks = new[]
        {
            Task.Run(() => AddTab<TransactionsTab>(_serviceProvider, "История транзакций")),
            Task.Run(() => AddTab<AccountTab>(_serviceProvider, "Настройка счета")),
            Task.Run(() => AddTab<UserProfileTab>(_serviceProvider, "Профиль")),
            Task.Run(() => AddTab<CategoriesTab>(_serviceProvider, "Категории транзакций")),
        };

        await Task.WhenAll(tasks);
    }

    private AuthorizedTabBase AddTab<TTab>(IServiceProvider serviceProvider, string header, bool isTabbarVisible = true) where TTab : ContentView
    {
        var tab = serviceProvider.GetRequiredService<TTab>();
        var tabModel = new AuthorizedTabBase() { Tab = tab, Header = header, IsTabbarVisible = isTabbarVisible };

        return _tabsCollection.AddOrUpdate(typeof(TTab), (type) => tabModel, (type, _) => tabModel);
    }
}
