namespace PurseAccountinng.Mobile.Presentation.Pages.Authorized.Account;

public partial class AccountTab : ContentView
{
    public AccountTab(AccountTabViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
        AccountWidget.BindingContext = viewModel;
        AccountAttributesWidget.BindingContext = viewModel;
    }

    private void OnAutoPlannedTransactionSwiped(object? sender, Components.Transactions.AutoPlannedTransactionSwipedEventArgs e)
    {
        if (BindingContext is AccountTabViewModel viewModel)
        {
            _ = viewModel.DeleteAutoPlannedTransactionAsync(e.PlannedTransactionSettingInfo);
        }
    }
}
