using PurseAccountinng.Mobile.Presentation.Components;
using PurseAccountinng.Mobile.Presentation.Pages.Authorized.Accounting;
using PurseAccountinng.Mobile.Presentation.Pages.Authorized.Distribution;
using PurseAccountinng.Mobile.Presentation.Pages.Authorized.Transactions;
using PurseAccounting.Mobile.Application.Distribution;
using Animation = Microsoft.Maui.Controls.Animation;

namespace PurseAccountinng.Mobile.Presentation.Pages.Authorized;

public partial class AuthorizedPage : ContentPage
{
    private const double _sheetHeight = 320;
    private const int _directionStabilityLimit = 3;

    private readonly Queue<SwipeDirection> _directionHistory = new(_directionStabilityLimit);

    private readonly AuthorizedTabBase _accountingTab;

    private AuthorizedTabBase? _transactionsTab;
    private AuthorizedTabBase? _accountTab;
    private AuthorizedTabBase? _userProfileTab;
    private AuthorizedTabBase? _categoriesTab;
    private AuthorizedTabBase? _distributionTab;

    private TabIconButton? _lastActiveTabButton;

    private bool _isSheetOpen = false;
    private bool _isDragging = false;
    private double _currentY = 0;
    private double _lastTotalY = 0;
    private SwipeDirection? _lastStableDirection = null;

    private static double ScreenHeight => DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density;

    private TabIconButton? LastActiveTabButton
    {
        get
        {
            return _lastActiveTabButton;
        }

        set
        {
            if (_lastActiveTabButton is not null)
                _lastActiveTabButton.IsActive = false;

            if (value is not null)
                value.IsActive = true;

            _lastActiveTabButton = value;
        }
    }

    private double SheetHiddenPosition => ScreenHeight - TabbarGrid.Height;

    private double SheetOpenPosition => SheetHiddenPosition - _sheetHeight;

    public AuthorizedPage(IServiceProvider serviceProvider)
    {
        InitializeComponent();

        _accountingTab = new() { Header = "Добавить транзакцию", Tab = new AccountingTab(serviceProvider) };
        _ = LoadTabsContentAsync(serviceProvider);

        BindingContext = ActivatorUtilities.CreateInstance<AuthorizedViewModel>(serviceProvider);

        _distributionTab = new() { Header = "Распределение остатка", Tab = new DistributionTab(serviceProvider) };
        _ = InitializeActiveTabAsync(serviceProvider);
        //SetActiveTab(_accountingTab);
        LastActiveTabButton = BtnHome;
    }

    private async Task InitializeActiveTabAsync(IServiceProvider serviceProvider)
    {
        var distributionService = serviceProvider.GetRequiredService<IDistributionService>();
        var availableUserChoiceDistributionStrategy = await distributionService.GetAvailableUserChoiceDistributionStrategy(CancellationToken.None);

        if (availableUserChoiceDistributionStrategy is not null)
            SetActiveTab(_distributionTab);
        else
            SetActiveTab(_accountingTab);
    }

    private async Task LoadTabsContentAsync(IServiceProvider serviceProvider)
    {
        var tasks = new[]
        {
            Task.Run(() => _transactionsTab = new() { Header = "История транзакций", Tab = new TransactionsTab(serviceProvider) }),
            Task.Run(() => _accountTab = new() { Header = "Настройка счета", Tab = new Account.AccountTab(serviceProvider) }),
            Task.Run(() => _userProfileTab = new() { Header = "Профиль", Tab = new UserProfileTab() }),
            Task.Run(() => _categoriesTab = new() { Header = "Категории транзакций", Tab = new CategoriesTab() }),
        };

        await Task.WhenAll(tasks);
    }

    private void SetActiveTab(AuthorizedTabBase authorizedTab)
    {
        TabHeader.Text = authorizedTab.Header;
        MainContent.Content = authorizedTab.Tab;

        if (_isSheetOpen)
            CloseSheetAnimated();
    }

    private void OnAccountingTabClicked(object sender, EventArgs e)
    {
        SetActiveTab(_accountingTab);
        LastActiveTabButton = BtnHome;
    }

    private void OnTransactionsTabClicked(object sender, EventArgs e)
    {
        if (_transactionsTab is null)
            return;

        SetActiveTab(_transactionsTab);
        LastActiveTabButton = BtnList;
    }

    private void OnAccountTabClicked(object sender, EventArgs e)
    {
        if (_accountTab is null)
            return;

        SetActiveTab(_accountTab);
        LastActiveTabButton = BtnAccount;
    }

    private void OnSettingsTabClicked(object sender, EventArgs e)
    {
        if (_isDragging)
            return;

        if (_isSheetOpen)
        {
            CloseSheetAnimated();
            LastActiveTabButtonActive(true);
        }
        else
        {
            LastActiveTabButtonActive(false);
            BtnSettings.IsActive = true;

            AbsoluteLayout.SetLayoutBounds(BottomSheet, new Rect(0, SheetHiddenPosition, 1, _sheetHeight)); // to prevent blinking
            OpenSheetAnimated();
        }
    }

    private void OnOverlayTapped(object sender, EventArgs e)
    {
        CloseSheetAnimated();
        LastActiveTabButtonActive(true);
    }

    private void OnDragHandlePanUpdated(object sender, PanUpdatedEventArgs e)
    {
        switch (e.StatusType)
        {
            case GestureStatus.Started:
                _isDragging = true;
                _currentY = AbsoluteLayout.GetLayoutBounds(BottomSheet).Y;
                _lastTotalY = e.TotalY;
                _directionHistory.Clear();
                _lastStableDirection = null;
                break;

            case GestureStatus.Running:
                if (!_isDragging)
                    return;

                var deltaY = e.TotalY - _lastTotalY;
                _lastTotalY = e.TotalY;

                if (Math.Abs(deltaY) < 1.0)
                    return;

                var currentDirection = deltaY > 0 ? SwipeDirection.Down : SwipeDirection.Up;

                _directionHistory.Enqueue(currentDirection);

                if (_directionHistory.Count > _directionStabilityLimit)
                {
                    _directionHistory.Dequeue();
                }
                else if (_directionHistory.Count == _directionStabilityLimit && _directionHistory.All(x => x == currentDirection))
                {
                    _lastStableDirection = currentDirection;

                    _currentY += deltaY;
                    _currentY = Math.Max(SheetOpenPosition, Math.Min(SheetHiddenPosition, _currentY));

                    AbsoluteLayout.SetLayoutBounds(BottomSheet, new Rect(0, _currentY, 1, _sheetHeight));
                }

                return;
            case GestureStatus.Completed:
            case GestureStatus.Canceled:
                if (!_isDragging)
                    return;

                _isDragging = false;
                _directionHistory.Clear();

                if (_lastStableDirection.HasValue && _lastStableDirection.Value == SwipeDirection.Up)
                {
                    OpenSheetAnimated();
                }
                else
                {
                    CloseSheetAnimated();
                    LastActiveTabButtonActive(true);
                }

                break;
        }
    }

    private async void OpenSheetAnimated()
    {
        _isSheetOpen = true;

        BottomSheet.IsVisible = true;
        Overlay.IsVisible = true;

        await Overlay.FadeTo(1, 100);

        var startY = AbsoluteLayout.GetLayoutBounds(BottomSheet).Y;
        var animation = new Animation(x => AbsoluteLayout.SetLayoutBounds(BottomSheet, new Rect(0, x, 1, _sheetHeight)), startY, SheetOpenPosition);

        animation.Commit(BottomSheet, "Open", 16, 300, Easing.SinOut);
    }

    private async void CloseSheetAnimated()
    {
        var startY = AbsoluteLayout.GetLayoutBounds(BottomSheet).Y;
        var animation = new Animation(x => AbsoluteLayout.SetLayoutBounds(BottomSheet, new Rect(0, x, 1, _sheetHeight)), startY, SheetHiddenPosition);

        animation.Commit(BottomSheet, "Close", 16, 200, Easing.SinIn, finished: (_, _) =>
        {
            Dispatcher.Dispatch(() =>
            {
                BottomSheet.IsVisible = false;
                Overlay.IsVisible = false;
                _isSheetOpen = false;
            });
        });

        await Overlay.FadeTo(100, 1);

        BtnSettings.IsActive = false;
    }

    private void OnProfileClicked(object sender, EventArgs e)
    {
        if (_userProfileTab is null)
            return;

        CloseSheetAnimated();
        SetActiveTab(_userProfileTab);
        LastActiveTabButton = null;
    }

    private void OnCategoriesClicked(object sender, EventArgs e)
    {
        if (_categoriesTab is null)
            return;

        CloseSheetAnimated();
        SetActiveTab(_categoriesTab);
        LastActiveTabButton = null;
    }

    private void LastActiveTabButtonActive(bool isActive)
    {
        if (LastActiveTabButton is not null)
            LastActiveTabButton.IsActive = isActive;
    }
}
