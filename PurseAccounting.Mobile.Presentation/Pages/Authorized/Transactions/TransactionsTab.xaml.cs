using Microsoft.Maui.Controls;

namespace PurseAccountinng.Mobile.Presentation.Pages.Authorized.Transactions;

public partial class TransactionsTab : ContentView
{
    private bool _isLoadingMore;

    public TransactionsTab(IServiceProvider serviceProvider)
    {
        InitializeComponent();

        var transactionsTabViewModel = ActivatorUtilities.CreateInstance<TransactionsTabViewModel>(serviceProvider);
        BindingContext = transactionsTabViewModel;

        if (transactionsTabViewModel is TransactionsTabViewModel vm)
        {
            vm.PropertyChanged += ViewModel_PropertyChanged;
        }
    }

    private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(TransactionsTabViewModel.GroupedTransactions))
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                TransactionsCollectionView?.ScrollTo(0, 0, ScrollToPosition.Start, false);
            });
        }
    }

    private async void OnLoadMoreRequested(object? sender, EventArgs e)
    {
        if (_isLoadingMore)
            return;

        if (BindingContext is not TransactionsTabViewModel viewModel)
            return;

        if (!viewModel.CanLoadMore)
            return;

        _isLoadingMore = true;

        try
        {
            await Task.Delay(100);
            viewModel.LoadMoreGroups();
        }
        finally
        {
            await Task.Delay(50);
            _isLoadingMore = false;
        }
    }
}
