using System.Windows.Input;
using Animation = Microsoft.Maui.Controls.Animation;

namespace PurseAccountinng.Mobile.Presentation.Pages.Authorized;

public partial class AuthorizedPage : ContentPage
{
    private const double _sheetHeight = 310;
    private const int _directionStabilityWindow = 3;

    private readonly Queue<bool> _directionHistory = new(_directionStabilityWindow);

    private bool _isSheetOpen = false;
    private bool _isDragging = false;
    private double _currentY = 0;
    private double _lastTotalY = 0;
    private bool? _lastStableDirection = null; // true = вниз, false = вверх

    private double ScreenHeight => DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density;

    public AuthorizedPage()
    {
        InitializeComponent();
    }

    private void OnAccountingTabClicked(object sender, EventArgs e)
    {
        MainContent.Content = new AccountingTab();
        TabHeader.Text = "Добавить транзакцию";
        UpdateTabSelection(BtnHome);
    }

    private void OnTransactionsTabClicked(object sender, EventArgs e)
    {
        MainContent.Content = new TransactionsTab();
        TabHeader.Text = "История транзакций";
        UpdateTabSelection(BtnList);
    }

    private void OnAccountTabClicked(object sender, EventArgs e)
    {
        MainContent.Content = new AccountTab();
        TabHeader.Text = "Настройка счета";
        UpdateTabSelection(BtnAccount);
    }

    private void UpdateTabSelection(ImageButton activeButton)
    {
        BtnHome.Source = BtnHome == activeButton ? "home_active.svg" : "home_inactive.svg";
        BtnList.Source = BtnList == activeButton ? "list_active.svg" : "list_inactive.svg";
        BtnAccount.Source = BtnAccount == activeButton ? "account_active.svg" : "account_inactive.svg";
        BtnSettings.Source = BtnSettings == activeButton ? "settings_active.svg" : "settings_inactive.svg";
    }

    private void OnOverlayTapped(object sender, EventArgs e)
    {
        CloseSheetAnimated();
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

                var currentDirection = deltaY > 0;

                _directionHistory.Enqueue(currentDirection);
                if (_directionHistory.Count > _directionStabilityWindow)
                    _directionHistory.Dequeue();

                if (_directionHistory.Count == _directionStabilityWindow)
                {
                    if (_directionHistory.All(d => d == currentDirection))
                    {
                        _lastStableDirection = currentDirection;
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }

                _currentY += deltaY;

                var minY = ScreenHeight - _sheetHeight;
                var maxY = ScreenHeight;

                _currentY = Math.Max(minY, Math.Min(maxY, _currentY));

                AbsoluteLayout.SetLayoutBounds(BottomSheet, new Rect(0, _currentY, 1, _sheetHeight));
                break;
            case GestureStatus.Completed:
            case GestureStatus.Canceled:
                if (!_isDragging)
                    return;

                _isDragging = false;
                _directionHistory.Clear();

                if (_lastStableDirection.HasValue && _lastStableDirection.Value)
                {
                    CloseSheetAnimated();
                }
                else
                {
                    OpenSheetAnimated();
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
        var endY = ScreenHeight - _sheetHeight;

        var animation = new Animation(x => AbsoluteLayout.SetLayoutBounds(BottomSheet, new Rect(0, x, 1, _sheetHeight)), startY, endY);

        animation.Commit(BottomSheet, "Open", 16, 300, Easing.SinOut);
    }

    private async void CloseSheetAnimated()
    {
        var startY = AbsoluteLayout.GetLayoutBounds(BottomSheet).Y;
        var endY = ScreenHeight;

        var animation = new Animation(x => AbsoluteLayout.SetLayoutBounds(BottomSheet, new Rect(0, x, 1, _sheetHeight)), startY, endY);

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
    }

    private void OnShowSettingsSheet(object sender, EventArgs e)
    {
        if (_isSheetOpen || _isDragging)
            return;

        AbsoluteLayout.SetLayoutBounds(BottomSheet, new Rect(0, ScreenHeight, 1, _sheetHeight)); // to prevent blinking
        OpenSheetAnimated();
    }

    private void OnProfileClicked(object sender, EventArgs e)
    {
        CloseSheetAnimated();
    }

    private void OnCategoriesClicked(object sender, EventArgs e)
    {
        CloseSheetAnimated();
    }

    private void OnLogoutClicked(object sender, EventArgs e)
    {
        CloseSheetAnimated();
    }
}
