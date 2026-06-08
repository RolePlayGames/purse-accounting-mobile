namespace PurseAccountinng.Mobile.Presentation.Pages.Authorized.Account;

public partial class AccountTab : ContentView
{
    public AccountTab(AccountAttributesWidgetViewModel viewModel)
    {
        InitializeComponent();

        AccountWidget.BindingContext = viewModel;
        AccountAttributesWidget.BindingContext = viewModel;
    }
}
