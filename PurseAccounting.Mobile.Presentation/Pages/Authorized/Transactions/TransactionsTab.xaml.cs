namespace PurseAccountinng.Mobile.Presentation.Pages.Authorized.Transactions;

public partial class TransactionsTab : ContentView
{
    public TransactionsTab(TransactionsTabViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }
}
