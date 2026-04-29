using PurseAccounting.Mobile.Infrastructure.TransactionCategories;
using PurseAccounting.Mobile.Infrastructure.Transactions;
using PurseAccountinng.Mobile.Presentation.Colors;
using PurseAccountinng.Mobile.Presentation.Extensions;

namespace PurseAccountinng.Mobile.Presentation.Components.Transactions;

public partial class TransactionRow : ContentView
{
    public static readonly BindableProperty TransactionProperty =
        BindableProperty.Create(nameof(Transaction), typeof(TransactionInfo?), typeof(TransactionRow), default(TransactionInfo?), propertyChanged: OnTransactionOrCategoriesChanged);

    public static readonly BindableProperty CategoriesProperty =
        BindableProperty.Create(nameof(Categories), typeof(IReadOnlyDictionary<long, TransactionCategoryDto>), typeof(TransactionRow), default(IReadOnlyDictionary<long, TransactionCategoryDto>), propertyChanged: OnTransactionOrCategoriesChanged);

    public static readonly BindableProperty CircleColorProperty =
        BindableProperty.Create(nameof(CircleColor), typeof(Brush), typeof(TransactionRow), new SolidColorBrush(Microsoft.Maui.Graphics.Colors.Gray));

    public static readonly BindableProperty AmountTextProperty =
        BindableProperty.Create(nameof(AmountText), typeof(string), typeof(TransactionRow), string.Empty);

    public static readonly BindableProperty AmountTextColorProperty =
        BindableProperty.Create(nameof(AmountTextColor), typeof(Color), typeof(TransactionRow), Microsoft.Maui.Graphics.Colors.Black);

    public static readonly BindableProperty TransactionTypeTextProperty =
        BindableProperty.Create(nameof(TransactionTypeText), typeof(string), typeof(TransactionRow), string.Empty);

    public event EventHandler<TransactionSwipedEventArgs>? TransactionSwiped;

    public TransactionInfo? Transaction
    {
        get => (TransactionInfo?)GetValue(TransactionProperty);
        set => SetValue(TransactionProperty, value);
    }

    public IReadOnlyDictionary<long, TransactionCategoryDto> Categories
    {
        get => (IReadOnlyDictionary<long, TransactionCategoryDto>)GetValue(CategoriesProperty);
        set => SetValue(CategoriesProperty, value);
    }

    public Brush CircleColor
    {
        get => (Brush)GetValue(CircleColorProperty);
        set => SetValue(CircleColorProperty, value);
    }

    public string AmountText
    {
        get => (string)GetValue(AmountTextProperty);
        set => SetValue(AmountTextProperty, value);
    }

    public Color AmountTextColor
    {
        get => (Color)GetValue(AmountTextColorProperty);
        set => SetValue(AmountTextColorProperty, value);
    }

    public string TransactionTypeText
    {
        get => (string)GetValue(TransactionTypeTextProperty);
        set => SetValue(TransactionTypeTextProperty, value);
    }

    public TransactionRow()
    {
        InitializeComponent();
        SetupSwipeEvents();
        StartTrackingSwipePosition();
    }

    private void StartTrackingSwipePosition()
    {
        // Запускаем фоновую задачу для отслеживания позиции контента
        _ = TrackSwipePositionAsync();
    }

    private async System.Threading.Tasks.Task TrackSwipePositionAsync()
    {
        double lastOffset = 0;
        
        while (true)
        {
            try
            {
                // Получаем текущее смещение контента при свайпе
                var currentOffset = ContentContainer.TranslationX;
                
                // Если смещение изменилось значительно
                if (Math.Abs(currentOffset - lastOffset) > 0.5)
                {
                    lastOffset = currentOffset;
                    
                    // Проверяем порог
                    if (SwipeContainer.Width > 0)
                    {
                        double threshold = SwipeContainer.Width * 0.5;
                        _shouldPerformAction = Math.Abs(currentOffset) >= threshold;
                        _swipeOffset = currentOffset;
                    }
                }
            }
            catch
            {
                // Игнорируем ошибки
            }
            
            await System.Threading.Tasks.Task.Delay(16); // ~60 FPS
        }
    }

    private void SetupSwipeEvents()
    {
        SwipeContainer.SwipeEnded += OnSwipeEnded;
    }

    private double _swipeOffset;
    private bool _shouldPerformAction;

    private void OnSwipeEnded(object? sender, SwipeEndedEventArgs e)
    {
        if (_shouldPerformAction && Transaction.HasValue)
        {
            // Открываем свайп полностью и выполняем действие
            SwipeContainer.Open(OpenSwipeItem.RightItems, animated: false);
            TransactionSwiped?.Invoke(this, new TransactionSwipedEventArgs(Transaction.Value));
        }
        else
        {
            // Закрываем свайп без выполнения действия
            SwipeContainer.Close(animated: true);
        }
        
        ContentContainer.Background = App.Current?.Resources.GetColor("WorkBackground");
        _shouldPerformAction = false;
    }

    private static void OnTransactionOrCategoriesChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TransactionRow row)
        {
            row.UpdateProperties();
        }
    }

    private static string FormatAmount(double amount)
    {
        var rubles = (long)amount;
        var kopecks = (int)(((amount - rubles) * 100) + 0.5);

        if (kopecks == 0)
            return $"{rubles:N0} ₽";

        return $"{rubles:N0},{kopecks:D2} ₽";
    }

    private void UpdateProperties()
    {
        if (!Transaction.HasValue || Categories is null || Categories.Count == 0)
            return;

        var transaction = Transaction.Value;
        
        if (Categories.TryGetValue(transaction.TransactionCategoryID, out var category) && ColorsMap.Map.TryGetValue(category.ColorID, out var color))
            CircleColor = new SolidColorBrush(color);
        else
            CircleColor = new SolidColorBrush(Microsoft.Maui.Graphics.Colors.Gray);

        var amount = transaction.Amount;
        var amountInRubles = amount / 100.0;
        var amountAbs = Math.Abs(amountInRubles);

        if (amount >= 0)
            AmountText = FormatAmount(amountAbs);
        else
            AmountText = $"+ {FormatAmount(amountAbs)}";

        TransactionTypeText = transaction.ChangeAmountType == "Daily" ? "Ежедневная" : "Общая";
    }
}

public class TransactionSwipedEventArgs : EventArgs
{
    public TransactionInfo Transaction { get; }

    public TransactionSwipedEventArgs(TransactionInfo transaction)
    {
        Transaction = transaction;
    }
}
