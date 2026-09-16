namespace PurseAccountinng.Mobile.Presentation.Pages.Authorized.Account;

public partial class AccountTab : ContentView
{
    public AccountTab(AccountTabViewModel viewModel, AccountAttributesWidgetViewModel widgetViewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
        AccountWidget.BindingContext = widgetViewModel;
        AccountAttributesWidget.BindingContext = widgetViewModel;
    }

    private void OnAutoPlannedTransactionSwiped(object? sender, Components.PlannedTransactions.AutoPlannedTransactionSwipedEventArgs e)
    {
        if (BindingContext is AccountTabViewModel viewModel)
        {
            _ = viewModel.DeleteAutoPlannedTransactionAsync(e.PlannedTransactionSettingInfo);
        }
    }
}
