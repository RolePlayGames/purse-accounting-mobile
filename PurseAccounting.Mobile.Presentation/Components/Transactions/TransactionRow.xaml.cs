using Microsoft.Maui.Controls.Shapes;
using PurseAccounting.Mobile.Infrastructure.TransactionCategories;
using PurseAccounting.Mobile.Infrastructure.Transactions;
using PurseAccountinng.Mobile.Presentation.Colors;
using PurseAccountinng.Mobile.Presentation.Extensions;
using PurseAccountinng.Mobile.Presentation.Services.Utils;

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

    public event EventHandler<TransactionSwipedEventArgs>? SwipeCompleted;

    private const int _maxDirectionHistory = 3;
    private const double _cornerRadius = 10; // pixels

    private static RoundRectangleGeometry? _contentContainerNormalRectangle;
    private static RoundRectangleGeometry? _contentContainerRoundedRectangle;

    private readonly Queue<bool> _swipeDirections = new();
    private double? _lastOffset = null;

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

    private RoundRectangleGeometry ContentContainerNormalRectangle => _contentContainerNormalRectangle ??= new()
    {
        Rect = new(0d, 0d, ContentContainer.Width, ContentContainer.Height),
    };

    private RoundRectangleGeometry ContentContainerRoundedRectangle => _contentContainerRoundedRectangle ??= new()
    {
        Rect = new(0d, 0d, ContentContainer.Width, ContentContainer.Height),
        CornerRadius = new(0, _cornerRadius, 0, _cornerRadius),
    };

    public TransactionRow()
    {
        InitializeComponent();
        SetupSwipeGesture();
    }

    private static void OnTransactionOrCategoriesChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TransactionRow row)
        {
            row.UpdateProperties();
        }
    }

    private void SetupSwipeGesture()
    {
        SwipeContainer.SwipeStarted += OnSwipeStarted;
        SwipeContainer.SwipeChanging += OnSwipeChanging;
        SwipeContainer.SwipeEnded += OnSwipeEnded;
    }

    private void OnSwipeStarted(object? sender, SwipeStartedEventArgs e)
    {
        ContentContainer.Background = App.Current?.Resources.GetColor("LightBlue");
        RoundContentContainerClip(true);
        _swipeDirections.Clear();
    }

    private void OnSwipeChanging(object? sender, SwipeChangingEventArgs e)
    {
        if (!_lastOffset.HasValue)
        {
            _lastOffset = e.Offset;
            return;
        }

        var isLeft = _lastOffset >= e.Offset;
        _lastOffset = e.Offset;

        _swipeDirections.Enqueue(isLeft);

        if (_swipeDirections.Count > _maxDirectionHistory)
        {
            _swipeDirections.Dequeue();
        }
    }

    private void OnSwipeEnded(object? sender, SwipeEndedEventArgs e)
    {
        var allLeft = _swipeDirections.Count > 0 && _swipeDirections.All(d => d);

        if (allLeft && Transaction.HasValue)
        {
            SwipeContainer.Open(OpenSwipeItem.RightItems, false);
            TransactionSwiped?.Invoke(this, new TransactionSwipedEventArgs(Transaction.Value));
            SwipeCompleted?.Invoke(this, new TransactionSwipedEventArgs(Transaction.Value));
        }
        else
        {
            SwipeContainer.Close(true);
            ContentContainer.Background = App.Current?.Resources.GetColor("WorkBackground");
            RoundContentContainerClip(false);
        }

        _lastOffset = null;
        _swipeDirections.Clear();
    }

    private void RoundContentContainerClip(bool isSwiping)
    {
        if (ContentContainer.Width <= 0)
            return;

        ContentContainer.Clip = isSwiping ? ContentContainerRoundedRectangle : ContentContainerNormalRectangle;
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
        var formattedAmount = AmountFormatter.FormatAmount(Math.Abs(amount));
        var amountSign = amount >= 0 ? '+' : '-';

        AmountText = $"{amountSign} {formattedAmount} ₽";
        AmountTextColor = (amount >= 0 ? App.Current?.Resources.GetColor("TransactionPositive") : App.Current?.Resources.GetColor("Gray1")) ?? AmountTextColor;

        TransactionTypeText = transaction.ChangeAmountType == "Daily" ? "Ежедневная" : "Общая";
    }
}
