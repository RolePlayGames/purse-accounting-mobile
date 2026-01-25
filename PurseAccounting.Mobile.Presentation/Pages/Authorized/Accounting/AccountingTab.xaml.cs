namespace PurseAccountinng.Mobile.Presentation.Pages.Authorized.Accounting;

public partial class AccountingTab : ContentView
{
    public AccountingTab(IServiceProvider serviceProvider)
    {
        InitializeComponent();

        AccountWidget.BindingContext = ActivatorUtilities.CreateInstance<AccountWidgetViewModel>(serviceProvider);
        TomorrowAmountWidget.BindingContext = ActivatorUtilities.CreateInstance<TomorrowAmountWidgetViewModel>(serviceProvider);
    }
}
