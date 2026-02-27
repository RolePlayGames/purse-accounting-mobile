namespace PurseAccountinng.Mobile.Presentation.Pages.Authorized.Account;

public partial class AccountTab : ContentView
{
    public AccountTab(IServiceProvider serviceProvider)
    {
        InitializeComponent();

        var accountAttributesWidgetViewModel = ActivatorUtilities.CreateInstance<AccountAttributesWidgetViewModel>(serviceProvider);
        AccountWidget.BindingContext = accountAttributesWidgetViewModel;
        AccountAttributesWidget.BindingContext = accountAttributesWidgetViewModel;
    }
}
