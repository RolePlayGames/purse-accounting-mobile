using PurseAccounting.Mobile.Infrastructure.TransactionCategories;
using PurseAccounting.Mobile.Infrastructure.Transactions;
using PurseAccountinng.Mobile.Presentation.Colors;

namespace PurseAccountinng.Mobile.Presentation.Components.Transactions;

public partial class TransactionRow : ContentView
{
    public static readonly BindableProperty TransactionProperty =
        BindableProperty.Create(nameof(Transaction), typeof(TransactionInfo?), typeof(TransactionRow), default(TransactionInfo?), propertyChanged: OnTransactionOrCategoriesChanged);

    public static readonly BindableProperty CategoriesProperty =
        BindableProperty.Create(nameof(Categories), typeof(IReadOnlyCollection<TransactionCategoryDto>), typeof(TransactionRow), default(IReadOnlyCollection<TransactionCategoryDto>), propertyChanged: OnTransactionOrCategoriesChanged);

    public static readonly BindableProperty CircleColorProperty =
        BindableProperty.Create(nameof(CircleColor), typeof(Brush), typeof(TransactionRow), new SolidColorBrush(Microsoft.Maui.Graphics.Colors.Gray));

    public static readonly BindableProperty AmountTextProperty =
        BindableProperty.Create(nameof(AmountText), typeof(string), typeof(TransactionRow), string.Empty);

    public static readonly BindableProperty AmountTextColorProperty =
        BindableProperty.Create(nameof(AmountTextColor), typeof(Color), typeof(TransactionRow), Microsoft.Maui.Graphics.Colors.Black);

    public static readonly BindableProperty TransactionTypeTextProperty =
        BindableProperty.Create(nameof(TransactionTypeText), typeof(string), typeof(TransactionRow), string.Empty);

    private bool _isSwiping;
    private double _startX;
    private double _currentTranslationX;
    private const double SwipeThreshold = 0.5; // 50% ширины для подтверждения свайпа

    public TransactionInfo? Transaction
    {
        get => (TransactionInfo?)GetValue(TransactionProperty);
        set => SetValue(TransactionProperty, value);
    }

    public IReadOnlyCollection<TransactionCategoryDto> Categories
    {
        get => (IReadOnlyCollection<TransactionCategoryDto>)GetValue(CategoriesProperty);
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

    public event EventHandler<TransactionSwipedEventArgs>? TransactionSwiped;

    public TransactionRow()
    {
        InitializeComponent();
        SetupPanGesture();
    }

    private void SetupPanGesture()
    {
        var panGesture = new PanGestureRecognizer();
        panGesture.PanUpdated += OnPanUpdated;
        ContentContainer.GestureRecognizers.Add(panGesture);
    }

    private void OnPanUpdated(object? sender, PanUpdatedEventArgs e)
    {
        switch (e.StatusType)
        {
            case GestureStatus.Started:
                _isSwiping = true;
                _startX = e.TotalX;
                _currentTranslationX = ContentContainer.TranslationX;
                break;

            case GestureStatus.Running:
                if (!_isSwiping) return;

                var deltaX = e.TotalX - _startX;
                
                // Разрешаем движение только влево
                if (deltaX < 0)
                {
                    var newTranslationX = Math.Max(deltaX, -MainGrid.Width);
                    ContentContainer.TranslationX = newTranslationX;
                    _currentTranslationX = newTranslationX;
                }
                break;

            case GestureStatus.Completed:
                if (!_isSwiping) return;
                _isSwiping = false;

                var parentWidth = MainGrid.Width;
                var swipeProgress = Math.Abs(_currentTranslationX) / parentWidth;

                if (swipeProgress >= SwipeThreshold)
                {
                    // Свайп завершен - убираем элемент
                    AnimateSwipeOut(-parentWidth);
                }
                else
                {
                    // Возвращаем на место
                    AnimateBackToPosition();
                }
                break;

            case GestureStatus.Canceled:
                _isSwiping = false;
                AnimateBackToPosition();
                break;
        }
    }

    private async void AnimateSwipeOut(double targetX)
    {
        await ContentContainer.TranslateTo(targetX, 0, 150, Easing.CubicIn);
        
        // Вызываем событие о том, что элемент был свайпнут
        TransactionSwiped?.Invoke(this, new TransactionSwipedEventArgs(Transaction));
        
        // Скрываем контент полностью
        ContentContainer.IsVisible = false;
    }

    private async void AnimateBackToPosition()
    {
        await ContentContainer.TranslateTo(0, 0, 200, Easing.CubicOut);
        _currentTranslationX = 0;
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
        var category = Categories.FirstOrDefault(c => c.ID == transaction.TransactionCategoryID);

        if (category is not null && ColorsMap.Map.TryGetValue(category.ColorID, out var color))
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
    public TransactionInfo? Transaction { get; }

    public TransactionSwipedEventArgs(TransactionInfo? transaction)
    {
        Transaction = transaction;
    }
}
