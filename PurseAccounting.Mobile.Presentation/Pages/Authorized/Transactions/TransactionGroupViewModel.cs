using PurseAccounting.Mobile.Application.Transactions;
using PurseAccounting.Mobile.Infrastructure.Transactions;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PurseAccountinng.Mobile.Presentation.Pages.Authorized.Transactions;

public class TransactionGroupViewModel : ReactiveObject
{
    private readonly TransactionGroup _group;
    private readonly ITransactionService _transactionService;
    private readonly ObservableCollection<TransactionInfo> _transactions;
    private string _dateText = string.Empty;

    public event Action<TransactionGroupViewModel>? GroupBecameEmpty;

    public TransactionGroupViewModel(
        TransactionGroup group,
        ITransactionService transactionService)
    {
        _group = group;
        _transactionService = transactionService;
        _transactions = new ObservableCollection<TransactionInfo>(group.Transactions);

        CancelTransactionCommand = ReactiveCommand.CreateFromTask<TransactionInfo>(HandleCancelTransaction);

        UpdateDateText();
    }

    public string DateText
    {
        get => _dateText;
        private set => this.RaiseAndSetIfChanged(ref _dateText, value);
    }

    public ObservableCollection<TransactionInfo> Transactions => _transactions;

    public ICommand CancelTransactionCommand { get; }

    private async Task HandleCancelTransaction(TransactionInfo transaction)
    {
        var changeAmountType = transaction.ChangeAmountType == "Daily"
            ? TransactionChangeAmountType.Daily
            : TransactionChangeAmountType.Total;

        var result = await _transactionService.CancelTransaction(transaction.ID, changeAmountType, CancellationToken.None);

        if (result)
        {
            var transactionToRemove = _transactions.FirstOrDefault(t => t.ID == transaction.ID);
            if (transactionToRemove != default)
            {
                _transactions.Remove(transactionToRemove);

                if (_transactions.Count == 0)
                {
                    GroupBecameEmpty?.Invoke(this);
                }
            }
        }
    }

    private void UpdateDateText()
    {
        var today = DateTime.Today;
        var culture = System.Globalization.CultureInfo.GetCultureInfo("ru-RU");
        var groupDate = _group.GroupDate;

        if (groupDate.Date == today)
        {
            DateText = $"{groupDate.ToString("d MMMM", culture)}, сегодня";
        }
        else if (groupDate.Date == today.AddDays(-1))
        {
            DateText = $"{groupDate.ToString("d MMMM", culture)}, вчера";
        }
        else if (groupDate.Year == today.Year)
        {
            DateText = $"{groupDate.ToString("d MMMM", culture)}, {groupDate.ToString("dddd", culture)}";
        }
        else
        {
            DateText = $"{groupDate.ToString("dd.MM.yyyy", culture)}, {groupDate.ToString("dddd", culture)}";
        }
    }
}
