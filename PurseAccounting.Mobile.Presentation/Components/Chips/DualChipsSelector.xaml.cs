namespace PurseAccountinng.Mobile.Presentation.Components.Chips;

public partial class DualChipsSelector : ContentView
{
    // Заголовок
    public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(DualChipsSelector), string.Empty);

    // Левый чип
    public static readonly BindableProperty LeftTextProperty = BindableProperty.Create(nameof(LeftText), typeof(string), typeof(DualChipsSelector), string.Empty);
    public static readonly BindableProperty LeftValueProperty = BindableProperty.Create(nameof(LeftValue), typeof(object), typeof(DualChipsSelector), null);

    // Правый чип
    public static readonly BindableProperty RightTextProperty = BindableProperty.Create(nameof(RightText), typeof(string), typeof(DualChipsSelector), string.Empty);
    public static readonly BindableProperty RightValueProperty = BindableProperty.Create(nameof(RightValue), typeof(object), typeof(DualChipsSelector), null);

    // Выбранное значение
    public static readonly BindableProperty SelectedValueProperty = BindableProperty.Create(nameof(SelectedValue), typeof(object), typeof(DualChipsSelector), null, defaultBindingMode: BindingMode.TwoWay, propertyChanged: OnSelectedValueChanged);

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string LeftText
    {
        get => (string)GetValue(LeftTextProperty);
        set => SetValue(LeftTextProperty, value);
    }

    public object LeftValue
    {
        get => GetValue(LeftValueProperty);
        set => SetValue(LeftValueProperty, value);
    }

    public string RightText
    {
        get => (string)GetValue(RightTextProperty);
        set => SetValue(RightTextProperty, value);
    }

    public object RightValue
    {
        get => GetValue(RightValueProperty);
        set => SetValue(RightValueProperty, value);
    }

    public object SelectedValue
    {
        get => GetValue(SelectedValueProperty);
        set => SetValue(SelectedValueProperty, value);
    }

    public event EventHandler<ChipsValueChangedEventArgs>? ValueChanged;

    public DualChipsSelector()
    {
        InitializeComponent();

        LeftChip.ChipTapped += OnLeftChipTapped;
        RightChip.ChipTapped += OnRightChipTapped;

        // Инициализация: первый чип активен по умолчанию (гарантия, что один активен)
        LeftChip.IsActive = true;
        RightChip.IsActive = false;
        SelectedValue = LeftValue;
    }

    private void OnLeftChipTapped(object sender, EventArgs e)
    {
        if (!LeftChip.IsActive)
        {
            LeftChip.IsActive = true;
            RightChip.IsActive = false;
            SelectedValue = LeftValue;
            ValueChanged?.Invoke(this, new ChipsValueChangedEventArgs(SelectedValue));
        }
    }

    private void OnRightChipTapped(object sender, EventArgs e)
    {
        if (!RightChip.IsActive)
        {
            RightChip.IsActive = true;
            LeftChip.IsActive = false;
            SelectedValue = RightValue;
            ValueChanged?.Invoke(this, new ChipsValueChangedEventArgs(SelectedValue));
        }
    }

    private static void OnSelectedValueChanged(BindableObject bindable, object oldValue, object newValue)
    {
        // Опционально: поддержка двусторонней привязки извне
        var view = (DualChipsSelector)bindable;

        if (Equals(newValue, view.LeftValue))
        {
            view.LeftChip.IsActive = true;
            view.RightChip.IsActive = false;
        }
        else if (Equals(newValue, view.RightValue))
        {
            view.RightChip.IsActive = true;
            view.LeftChip.IsActive = false;
        }
    }
}

// Вспомогательный класс для события
public class ChipsValueChangedEventArgs : EventArgs
{
    public object NewValue { get; }

    public ChipsValueChangedEventArgs(object newValue) => NewValue = newValue;
}
