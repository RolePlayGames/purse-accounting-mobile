using AndroidX.Collection;
using CommunityToolkit.Maui.Extensions;
using System.Security.Principal;

namespace PurseAccountinng.Mobile.Presentation.Pages.Authorized;

public partial class AuthorizedPage : ContentPage
{
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

    /*
    private async void OnShowSettingsSheet(object sender, EventArgs e)
    {
        await this.ShowPopupAsync(new SettingsActionSheet());

        //await this.ShowPopupAsync(new AddTransactionPopup());
        // НЕ меняем MainContent!
    }
    */
    private double _screenHeight;

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _screenHeight = DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density;
    }

    private bool _isSheetOpen = false;/*
    private async void OnShowSettingsSheet(object sender, EventArgs e)
    {
        if (_isSheetOpen)
        {
            await CloseBottomSheet();
            return;
        }
        UpdateTabSelection(BtnSettings);

        _isSheetOpen = true;

        BottomSheet.TranslationY = _screenHeight;
        BottomSheet.IsVisible = true;
        BottomSheet.InputTransparent = false;
        Overlay.IsVisible = true;

        await Overlay.FadeTo(1, 100);
        await BottomSheet.TranslateTo(0, _screenHeight - BottomSheet.Height, 250);
    }

    private async void OnOverlayTapped(object sender, EventArgs e)
    {
        Console.WriteLine("Overlay tapped!"); // для отладки
        await CloseBottomSheet();
    }

    private async Task CloseBottomSheet()
    {
        if (!_isSheetOpen)
            return;

        // Ключевое: делаем шторку прозрачной ДО анимации
        BottomSheet.InputTransparent = true;

        await BottomSheet.TranslateTo(0, _screenHeight, 200);
        await Overlay.FadeTo(0, 100);

        BottomSheet.IsVisible = false;
        Overlay.IsVisible = false;
        _isSheetOpen = false;
    }*/
    private async void OnShowSettingsSheet(object sender, EventArgs e)
    {
        if (_isSheetOpen)
            return;
        _isSheetOpen = true;

        BottomSheet.IsVisible = true;
        Overlay.IsVisible = true;

        await Overlay.FadeTo(1, 100);

        // Анимация: Y от 2 → 1 (из-за экрана → к низу)
        var animation = new Animation(v =>
        {
            AbsoluteLayout.SetLayoutBounds(BottomSheet, new Rect(0, _screenHeight - BottomSheet.Height, 1, 200));
        });

        animation.Commit(BottomSheet, "SlideUp", 16, 250, Easing.SinOut);
    }

    private async void OnOverlayTapped(object sender, EventArgs e)
    {
        await CloseBottomSheet();
    }

    private async Task CloseBottomSheet()
    {
        if (!_isSheetOpen)
            return;

        var animation = new Animation(v =>
        {
            AbsoluteLayout.SetLayoutBounds(BottomSheet, new Rect(0, _screenHeight, 1, 200));
        });

        animation.Commit(BottomSheet, "SlideDown", 16, 200, Easing.SinIn, finished: (_,_) =>
        {
            Overlay.FadeTo(0, 100);
            Device.BeginInvokeOnMainThread(() =>
            {
                BottomSheet.IsVisible = false;
                Overlay.IsVisible = false;
                _isSheetOpen = false;
            });
        });
    }
}
