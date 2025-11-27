namespace PurseAccountinng.Mobile.Presentation.Pages.Authorized.Accounting;

public partial class AccountingTab : ContentView
{
    public AccountingTab(IServiceProvider serviceProvider)
    {
        InitializeComponent();

        Widget.BindingContext = ActivatorUtilities.CreateInstance<AccountWidgetViewModel>(serviceProvider);
    }
}
