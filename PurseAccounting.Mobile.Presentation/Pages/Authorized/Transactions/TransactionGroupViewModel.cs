using PurseAccounting.Mobile.Application.Transactions;
using PurseAccounting.Mobile.Infrastructure.Transactions;
using PurseAccountinng.Mobile.Presentation.Services.Notifications;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PurseAccountinng.Mobile.Presentation.Pages.Authorized.Transactions;

public class TransactionGroupViewModel : ReactiveObject, IDisposable
{
    private readonly INotificationService _notificationService;
    private readonly ITransactionService _transactionService;
    private string _dateText = string.Empty;
    private bool _disposed;

    public event Action<TransactionGroupViewModel>? GroupBecameEmpty;

    public string DateText
    {
        get => _dateText;
        private set => this.RaiseAndSetIfChanged(ref _dateText, value);
    }

    public ICommand CancelTransactionCommand { get; }

    public ObservableCollection<TransactionInfo> Transactions { get; }

    public TransactionGroupViewModel(TransactionGroup group, ITransactionService transactionService, INotificationService notificationService)
    {
        _transactionService = transactionService;
        _notificationService = notificationService;

        Transactions = new ObservableCollection<TransactionInfo>(group.Transactions);

        CancelTransactionCommand = ReactiveCommand.CreateFromTask<TransactionInfo>(HandleCancelTransaction);

        DateText = GetDateText(group.GroupDate);
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            GroupBecameEmpty = null;
            _disposed = true;
        }
    }

    private static string GetDateText(DateTime groupDate)
    {
        var today = DateTime.Today;
        var culture = System.Globalization.CultureInfo.GetCultureInfo("ru-RU");

        if (groupDate.Date == today)
        {
            return $"{groupDate.ToString("d MMMM", culture)}, сегодня";
        }
        else if (groupDate.Date == today.AddDays(-1))
        {
            return $"{groupDate.ToString("d MMMM", culture)}, вчера";
        }
        else if (groupDate.Year == today.Year)
        {
            return $"{groupDate.ToString("d MMMM", culture)}, {groupDate.ToString("dddd", culture)}";
        }
        else
        {
            return $"{groupDate.ToString("dd.MM.yyyy", culture)}, {groupDate.ToString("dddd", culture)}";
        }
    }

    private async Task HandleCancelTransaction(TransactionInfo transaction)
    {
        var changeAmountType = transaction.ChangeAmountType == "Daily"
            ? TransactionChangeAmountType.Daily
            : TransactionChangeAmountType.Total;

        var result = await _transactionService.CancelTransaction(transaction.ID, changeAmountType, CancellationToken.None);

        if (result)
        {
            _notificationService.ShowSuccess("Транзакция отменена");

            var transactionToRemove = Transactions.FirstOrDefault(t => t.ID == transaction.ID);

            if (transactionToRemove != default)
            {
                Transactions.Remove(transactionToRemove);

                if (Transactions.Count == 0)
                    GroupBecameEmpty?.Invoke(this);
            }
        }
        else
        {
            _notificationService.ShowError("Операция не удалась. Попробуйте позже");
        }
    }
}
