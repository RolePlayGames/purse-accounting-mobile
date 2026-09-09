using Microsoft.Maui.Controls.Shapes;
using PurseAccounting.Mobile.Infrastructure.PlannedTransactions.Settings;
using PurseAccounting.Mobile.Infrastructure.PlannedTransactions.Settings.Periods;
using PurseAccounting.Mobile.Infrastructure.TransactionCategories;
using PurseAccounting.Mobile.Infrastructure.Transactions;
using PurseAccountinng.Mobile.Presentation.Colors;
using PurseAccountinng.Mobile.Presentation.Extensions;
using PurseAccountinng.Mobile.Presentation.Services.Utils;

namespace PurseAccountinng.Mobile.Presentation.Components.Transactions;

public partial class AutoPlannedTransactionRow : ContentView
{
    public static readonly BindableProperty PlannedTransactionSettingInfoProperty =
        BindableProperty.Create(nameof(PlannedTransactionSettingInfo), typeof(PlannedTransactionSettingInfo), typeof(AutoPlannedTransactionRow), default(PlannedTransactionSettingInfo), propertyChanged: OnPlannedTransactionSettingInfoChanged);

    public static readonly BindableProperty CategoriesProperty =
        BindableProperty.Create(nameof(Categories), typeof(IReadOnlyDictionary<long, TransactionCategoryDto>), typeof(AutoPlannedTransactionRow), default(IReadOnlyDictionary<long, TransactionCategoryDto>), propertyChanged: OnCategoriesChanged);

    public static readonly BindableProperty CircleColorProperty =
        BindableProperty.Create(nameof(CircleColor), typeof(Brush), typeof(AutoPlannedTransactionRow), new SolidColorBrush(Microsoft.Maui.Graphics.Colors.Gray));

    public event EventHandler<AutoPlannedTransactionSwipedEventArgs>? SwipeCompleted;

    private const int _maxDirectionHistory = 3;
    private const double _cornerRadius = 10; // pixels

    private static readonly SolidColorBrush _defaultBrush = new(Microsoft.Maui.Graphics.Colors.Gray);

    private static RoundRectangleGeometry? _contentContainerNormalRectangle;
    private static RoundRectangleGeometry? _contentContainerRoundedRectangle;

    private readonly Queue<bool> _swipeDirections = new();
    private double? _lastOffset = null;

    public PlannedTransactionSettingInfo? PlannedTransactionSettingInfo
    {
        get => (PlannedTransactionSettingInfo?)GetValue(PlannedTransactionSettingInfoProperty);
        set => SetValue(PlannedTransactionSettingInfoProperty, value);
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

    private RoundRectangleGeometry ContentContainerNormalRectangle => _contentContainerNormalRectangle ??= new()
    {
        Rect = new(0d, 0d, ContentContainer.Width, ContentContainer.Height),
    };

    private RoundRectangleGeometry ContentContainerRoundedRectangle => _contentContainerRoundedRectangle ??= new()
    {
        Rect = new(0d, 0d, ContentContainer.Width, ContentContainer.Height),
        CornerRadius = new(0, _cornerRadius, 0, _cornerRadius),
    };

    private static void OnPlannedTransactionSettingInfoChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is AutoPlannedTransactionRow row)
        {
            row.UpdateFromPlannedTransactionSettingInfo();
        }
    }

    private static void OnCategoriesChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is AutoPlannedTransactionRow row)
        {
            row.UpdateCircleColor();
        }
    }

    public AutoPlannedTransactionRow()
    {
        InitializeComponent();
        SetupSwipeGesture();
        UpdateState();
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

        if (allLeft && PlannedTransactionSettingInfo is not null)
        {
            SwipeContainer.Open(OpenSwipeItem.RightItems, false);
            SwipeCompleted?.Invoke(this, new AutoPlannedTransactionSwipedEventArgs(PlannedTransactionSettingInfo));
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

    private void UpdateFromPlannedTransactionSettingInfo()
    {
        var info = PlannedTransactionSettingInfo;
        if (info is null)
            return;

        TitleLabel.Text = info.Name;
        IconContainer.IsVisible = info.IsAutomatic;
        DescriptionLabel.Text = PeriodDescriptionFormatter.GetDescription(info.Period);

        UpdateAmountProperties(info.ChangeType);
        UpdateCircleColor();
    }

    private void UpdateCircleColor()
    {
        var info = PlannedTransactionSettingInfo;

        if (info is null || Categories is null || Categories.Count == 0)
        {
            CircleElement.Fill = _defaultBrush;
            return;
        }

        if (Categories.TryGetValue(info.TransactionCategoryID, out var category) && ColorsMap.Map.TryGetValue(category.ColorID, out var color))
            CircleElement.Fill = new SolidColorBrush(color);
        else
            CircleElement.Fill = _defaultBrush;
    }

    private void UpdateAmountProperties(TransactionChangeType? changeType = null)
    {
        var info = PlannedTransactionSettingInfo;

        if (info is null)
        {
            AmountLabel.Text = string.Empty;
            AmountLabel.TextColor = Microsoft.Maui.Graphics.Colors.Black;
            return;
        }

        var amount = info.Amount;
        var formattedAmount = AmountFormatter.FormatAmount(Math.Abs(amount));

        var actualChangeType = changeType ?? (amount >= 0 ? TransactionChangeType.Income : TransactionChangeType.Withdrawal);
        var amountSign = actualChangeType == TransactionChangeType.Income ? '+' : '-';

        AmountLabel.Text = $"{amountSign} {formattedAmount} ₽";
        AmountLabel.TextColor = (actualChangeType == TransactionChangeType.Income
                ? App.Current?.Resources.GetColor("TransactionPositive")
                : App.Current?.Resources.GetColor("Gray1"))
            ?? Microsoft.Maui.Graphics.Colors.Black;
    }

    private void UpdateState()
    {
        BindingContext = this;
    }
}
