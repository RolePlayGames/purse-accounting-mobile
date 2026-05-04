namespace PurseAccountinng.Mobile.Presentation.Pages.Authorized.Transactions;

using System.Linq;

public partial class TransactionsTab : ContentView
{
    public TransactionsTab(IServiceProvider serviceProvider)
    {
        InitializeComponent();

        var transactionsTabViewModel = ActivatorUtilities.CreateInstance<TransactionsTabViewModel>(serviceProvider);
        BindingContext = transactionsTabViewModel;
    }

    private void OnLoadMoreRequested(object? sender, EventArgs e)
    {
        if (BindingContext is TransactionsTabViewModel viewModel && viewModel.HasMoreGroupsToLoad)
        {
            // Проверяем, что все отображенные группы полностью показаны (все транзакции видны)
            var allGroupsFullyDisplayed = viewModel.DisplayedGroups.All(g => 
            {
                // В текущей реализации TransactionGroup не имеет ограничения на отображение,
                // поэтому считаем, что все группы всегда полностью отображены
                return true;
            });

            if (allGroupsFullyDisplayed)
            {
                viewModel.LoadMoreGroups();
            }
        }
    }
}
