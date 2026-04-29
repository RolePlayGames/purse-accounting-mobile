using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;

namespace YourAppNamespace.Behaviors
{
    public class SwipeProgressBehavior : Behavior<SwipeView>
    {
        private SwipeView? _swipeView;
        private double _maxSwipeDistance = 0;
        private bool _thresholdReached;

        protected override void OnAttachedTo(SwipeView swipeView)
        {
            base.OnAttachedTo(swipeView);
            _swipeView = swipeView;

            swipeView.PropertyChanged += OnSwipeViewPropertyChanged;
            
            // После загрузки получаем размеры для расчета порога
            _swipeView.SizeChanged += OnSwipeViewSizeChanged;
        }

        private void OnSwipeViewSizeChanged(object? sender, EventArgs e)
        {
            if (_swipeView != null)
            {
                // Порог - половина ширины SwipeView
                _maxSwipeDistance = _swipeView.Width * 0.5;
            }
        }

        protected override void OnDetachingFrom(SwipeView swipeView)
        {
            swipeView.PropertyChanged -= OnSwipeViewPropertyChanged;
            swipeView.SizeChanged -= OnSwipeViewSizeChanged;
            base.OnDetachingFrom(swipeView);
        }

        private async void OnSwipeViewPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == SwipeView.StateProperty.PropertyName && _swipeView != null)
            {
                var newState = _swipeView.State;
                
                // Когда свайп начинает закрываться (возвращаться назад или завершаться)
                if (newState == SwipeState.Closed)
                {
                    // Проверяем, был ли достигнут порог перед закрытием
                    await Task.Delay(50); // Небольшая задержка чтобы убедиться что анимация завершилась
                    
                    OnSwipeEnded?.Invoke(this, new SwipeEndedEventArgs(_thresholdReached));
                    
                    // Сбрасываем флаг
                    _thresholdReached = false;
                }
                else if (newState == SwipeState.Open)
                {
                    // Если свайп полностью открыт - значит порог точно достигнут
                    _thresholdReached = true;
                }
            }
        }

        // Метод для вызова из кода при изменении прогресса свайпа
        // Можно вызывать из SwipeItemView через Binding
        public void UpdateSwipeProgress(double currentOffset)
        {
            if (_maxSwipeDistance > 0)
            {
                _thresholdReached = Math.Abs(currentOffset) >= _maxSwipeDistance;
            }
        }

        public event EventHandler<SwipeEndedEventArgs>? OnSwipeEnded;
    }

    public class SwipeEndedEventArgs : EventArgs
    {
        public bool ThresholdReached { get; }

        public SwipeEndedEventArgs(bool thresholdReached)
        {
            ThresholdReached = thresholdReached;
        }
    }
}
