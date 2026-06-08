namespace PurseAccountinng.Mobile.Presentation.Pages.Authorized.Accounting;

public partial class AccountingTab : ContentView
{
    public AccountingTab(AccountWidgetViewModel accountWidgetViewModel, TomorrowAmountWidgetViewModel tomorrowAmountWidgetViewModel, TransactionAttributesViewModel transactionAttributesViewModel)
    {
        InitializeComponent();

        AccountWidget.BindingContext = accountWidgetViewModel;
        TomorrowAmountWidget.BindingContext = tomorrowAmountWidgetViewModel;

        TransactionAttributesWidget.BindingContext = transactionAttributesViewModel;
        TransactionAmountInput.BindingContext = transactionAttributesViewModel;
    }
}
