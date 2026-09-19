using Microsoft.Maui.Controls.Shapes;
using PurseAccounting.Mobile.Infrastructure.PlannedTransactions.Settings;
using PurseAccounting.Mobile.Infrastructure.PlannedTransactions.Settings.Periods;
using PurseAccounting.Mobile.Infrastructure.TransactionCategories;
using PurseAccounting.Mobile.Infrastructure.Transactions;
using PurseAccountinng.Mobile.Presentation.Colors;
using PurseAccountinng.Mobile.Presentation.Extensions;
using PurseAccountinng.Mobile.Presentation.Services.Utils;

namespace PurseAccountinng.Mobile.Presentation.Components.PlannedTransactions;

public partial class PlannedTransactionSettingRow : ContentView
{
    public static readonly BindableProperty PlannedTransactionSettingInfoProperty =
        BindableProperty.Create(nameof(PlannedTransactionSettingInfo), typeof(PlannedTransactionSettingInfo), typeof(PlannedTransactionSettingRow), default(PlannedTransactionSettingInfo), propertyChanged: OnPlannedTransactionSettingInfoChanged);

    public static readonly BindableProperty CategoriesProperty =
        BindableProperty.Create(nameof(Categories), typeof(IReadOnlyDictionary<long, TransactionCategoryDto>), typeof(PlannedTransactionSettingRow), default(IReadOnlyDictionary<long, TransactionCategoryDto>), propertyChanged: OnCategoriesChanged);

    public static readonly BindableProperty CircleColorProperty =
        BindableProperty.Create(nameof(CircleColor), typeof(Brush), typeof(PlannedTransactionSettingRow), new SolidColorBrush(Microsoft.Maui.Graphics.Colors.Gray));

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
        Rect = new(0d, 0d, SwipeableContentBorder.Width, SwipeableContentBorder.Height),
    };

    private RoundRectangleGeometry ContentContainerRoundedRectangle => _contentContainerRoundedRectangle ??= new()
    {
        Rect = new(0d, 0d, SwipeableContentBorder.Width, SwipeableContentBorder.Height),
        CornerRadius = new(0, _cornerRadius, 0, _cornerRadius),
    };

    private static void OnPlannedTransactionSettingInfoChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is PlannedTransactionSettingRow row && newValue is PlannedTransactionSettingInfo newInfo)
        {
            row.UpdateFromPlannedTransactionSettingInfo(newInfo);
        }
    }

    private static void OnCategoriesChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is PlannedTransactionSettingRow row && newValue is IReadOnlyDictionary<long, TransactionCategoryDto> newCategories)
        {
            row.UpdateCircleColor(row.PlannedTransactionSettingInfo, newCategories);
        }
    }

    public PlannedTransactionSettingRow()
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
        SwipeableContentBorder.Background = App.Current?.Resources.GetColor("LightBlue");
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
            SwipeableContentBorder.Background = App.Current?.Resources.GetColor("WorkBackground");
            RoundContentContainerClip(false);
        }

        _lastOffset = null;
        _swipeDirections.Clear();
    }

    private void RoundContentContainerClip(bool isSwiping)
    {
        if (SwipeableContentBorder.Width <= 0)
            return;

        SwipeableContentBorder.Clip = isSwiping ? ContentContainerRoundedRectangle : ContentContainerNormalRectangle;
    }

    private void UpdateFromPlannedTransactionSettingInfo(PlannedTransactionSettingInfo? info)
    {
        if (info is null)
            return;

        TitleLabel.Text = info.Name;
        IconContainer.IsVisible = info.IsAutomatic;
        DescriptionLabel.Text = PeriodDescriptionFormatter.GetDescription(info.Period);

        UpdateAmountProperties(info.ChangeType, info.Amount);
        UpdateCircleColor(info, Categories);
    }

    private void UpdateCircleColor(PlannedTransactionSettingInfo? info, IReadOnlyDictionary<long, TransactionCategoryDto>? categories)
    {
        if (info is null || categories is null || categories.Count == 0)
        {
            CircleElement.Fill = _defaultBrush;
            return;
        }

        if (categories.TryGetValue(info.TransactionCategoryID, out var category) && ColorsMap.Map.TryGetValue(category.ColorID, out var color))
            CircleElement.Fill = new SolidColorBrush(color);
        else
            CircleElement.Fill = _defaultBrush;
    }

    private void UpdateAmountProperties(TransactionChangeType changeType, int amount)
    {
        var (text, textColor) = AmountFormatter.FormatTransactionAmount(amount, changeType);

        AmountLabel.Text = text;
        AmountLabel.TextColor = textColor;
    }

    private void UpdateState()
    {
        BindingContext = this;
    }
}
