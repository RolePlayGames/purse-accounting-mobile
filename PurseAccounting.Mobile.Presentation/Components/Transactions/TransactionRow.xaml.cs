using System.Windows.Input;

namespace PurseAccountinng.Mobile.Presentation.Components.Transactions;

public partial class TransactionRow : ContentView
{
    public static readonly BindableProperty ContentProperty =
        BindableProperty.Create(nameof(Content), typeof(View), typeof(TransactionRow), null,
            propertyChanged: OnContentChanged);

    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(TransactionRow), null);

    public static readonly BindableProperty CommandParameterProperty =
        BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(TransactionRow), null);

    private double _startX;
    private double _currentTranslationX;
    private bool _isSwipeInProgress;
    private bool _isPressed;

    public View? Content
    {
        get => (View?)GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    public ICommand? Command
    {
        get => (ICommand?)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public event EventHandler? Swiped;

    public TransactionRow()
    {
        InitializeComponent();
        SetupGestures();
    }

    private void SetupGestures()
    {
        var panGesture = new PanGestureRecognizer();
        panGesture.PanUpdated += OnPanUpdated;
        ContentGrid.GestureRecognizers.Add(panGesture);

        // Use TouchAction for press detection
        ContentGrid.TouchAction += OnTouchAction;
    }

    private static void OnContentChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TransactionRow row && newValue is View content)
        {
            row.ContentHost.Content = content;
        }
    }

    private void OnTouchAction(object? sender, TouchActionEventArgs e)
    {
        switch (e.Type)
        {
            case TouchActionType.Pressed:
                _isPressed = true;
                // Background is already LightBlue from XAML
                break;
            case TouchActionType.Released:
            case TouchActionType.Cancelled:
                _isPressed = false;
                break;
        }
    }

    private void OnPanUpdated(object? sender, PanUpdatedEventArgs e)
    {
        switch (e.StatusType)
        {
            case GestureStatus.Started:
                _startX = e.TotalX;
                _currentTranslationX = ContentGrid.TranslationX;
                _isSwipeInProgress = true;
                PurpleBackground.IsVisible = true;
                break;

            case GestureStatus.Running:
                HandlePanMovement(e.TotalX);
                break;

            case GestureStatus.Completed:
            case GestureStatus.Canceled:
                HandlePanEnd();
                break;
        }
    }

    private void HandlePanMovement(double totalX)
    {
        var deltaX = totalX - _startX;
        var newTranslationX = _currentTranslationX + deltaX;

        // Only allow moving left (negative values)
        if (newTranslationX > 0)
        {
            newTranslationX = 0;
        }

        ContentGrid.TranslationX = newTranslationX;
    }

    private void HandlePanEnd()
    {
        var halfWidth = -ContentGrid.Width / 2;
        
        if (ContentGrid.TranslationX < halfWidth)
        {
            // Swipe past the middle - complete the swipe
            CompleteSwipe();
        }
        else
        {
            // Not far enough - animate back
            AnimateBackToOrigin();
        }

        _isSwipeInProgress = false;
    }

    private void AnimateBackToOrigin()
    {
        ContentGrid.TranslateTo(0, 0, 150, Easing.CubicOut);
        PurpleBackground.IsVisible = false;
    }

    private async void CompleteSwipe()
    {
        // Animate off-screen to the left
        await ContentGrid.TranslateTo(-ContentGrid.Width, 0, 200, Easing.CubicOut);
        
        // Hide the content and show purple background
        ContentGrid.IsVisible = false;
        PurpleBackground.IsVisible = true;
        
        // Execute command if provided
        if (Command?.CanExecute(CommandParameter) == true)
        {
            Command.Execute(CommandParameter);
        }
        
        Swiped?.Invoke(this, EventArgs.Empty);
    }
}
