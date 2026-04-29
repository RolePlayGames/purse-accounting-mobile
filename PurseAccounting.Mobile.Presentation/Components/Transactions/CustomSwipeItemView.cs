using System.Diagnostics;

namespace PurseAccountinng.Mobile.Presentation.Components.Transactions;

public class CustomSwipeItemView : SwipeItemView
{
    public event EventHandler<CustomSwipeProgressEventArgs>? SwipeProgressChanged;
    public event EventHandler<CustomSwipeEndedEventArgs>? CustomSwipeEnded;

    private double _startX;
    private double _currentOffset;
    private bool _isSwiping;

    public CustomSwipeItemView()
    {
        var panGesture = new PanGestureRecognizer();
        panGesture.PanUpdated += OnPanUpdated;
        GestureRecognizers.Add(panGesture);
    }

    private void OnPanUpdated(object? sender, PanUpdatedEventArgs e)
    {
        switch (e.StatusType)
        {
            case GestureStatus.Started:
                _startX = e.TotalX;
                _currentOffset = 0;
                _isSwiping = true;
                break;

            case GestureStatus.Running:
                if (_isSwiping)
                {
                    _currentOffset = e.TotalX - _startX;
                    // Отрицательное значение при свайпе влево
                    SwipeProgressChanged?.Invoke(this, new CustomSwipeProgressEventArgs(_currentOffset));
                }
                break;

            case GestureStatus.Completed:
            case GestureStatus.Canceled:
                if (_isSwiping)
                {
                    _isSwiping = false;
                    CustomSwipeEnded?.Invoke(this, new CustomSwipeEndedEventArgs(_currentOffset));
                }
                break;
        }
    }
}

public class CustomSwipeProgressEventArgs : EventArgs
{
    public double Offset { get; }

    public CustomSwipeProgressEventArgs(double offset)
    {
        Offset = offset;
    }
}

public class CustomSwipeEndedEventArgs : EventArgs
{
    public double Offset { get; }

    public CustomSwipeEndedEventArgs(double offset)
    {
        Offset = offset;
    }
}
