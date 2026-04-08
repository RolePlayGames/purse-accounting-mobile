namespace PurseAccountinng.Mobile.Presentation.Pages.Authorized.Transactions;

public partial class TransactionsTab : ContentView
{
    public TransactionsTab(IServiceProvider serviceProvider)
    {
        InitializeComponent();

        var transactionsTabViewModel = ActivatorUtilities.CreateInstance<TransactionsTabViewModel>(serviceProvider);
        BindingContext = transactionsTabViewModel;
    }
}
