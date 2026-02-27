using PurseAccounting.Mobile.Infrastructure.TransactionCategories;
using PurseAccountinng.Mobile.Presentation.Colors;

namespace PurseAccountinng.Mobile.Presentation.Components.Categories;

public partial class CategorySelector : ContentView
{
    public static readonly BindableProperty HeaderTextProperty
        = BindableProperty.Create(nameof(HeaderText), typeof(string), typeof(CategorySelector), string.Empty);

    public static readonly BindableProperty ItemsSourceProperty
        = BindableProperty.Create(nameof(ItemsSource), typeof(IList<TransactionCategoryDto>), typeof(CategorySelector), null, propertyChanged: OnItemsSourceChanged);

    public static readonly BindableProperty SelectedItemIdProperty
        = BindableProperty.Create(nameof(SelectedItemId), typeof(long?), typeof(CategorySelector), null, BindingMode.TwoWay, propertyChanged: OnSelectedIdChanged);

    public event EventHandler<SelectedItemChangedEventArgs>? SelectedItemChanged;

    public string HeaderText
    {
        get => (string)GetValue(HeaderTextProperty);
        set => SetValue(HeaderTextProperty, value);
    }

    public IList<TransactionCategoryDto> ItemsSource
    {
        get => (IList<TransactionCategoryDto>)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public long? SelectedItemId
    {
        get => (long?)GetValue(SelectedItemIdProperty);
        set => SetValue(SelectedItemIdProperty, value);
    }

    public CategorySelector()
    {
        InitializeComponent();
    }

    private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is CategorySelector control)
            control.UpdateItems();
    }

    private static void OnSelectedIdChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is CategorySelector control && control.ItemsSource != null)
        {
            control.UpdateSelection((long?)newValue);
        }
    }

    private void UpdateItems()
    {
        ItemsContainer.Children.Clear();

        if (ItemsSource == null || ItemsSource.Count == 0)
            return;

        var initialId = SelectedItemId ?? (ItemsSource.FirstOrDefault(x => x.IsDefault) ?? ItemsSource.First()).ID;

        foreach (var item in ItemsSource)
        {
            var brush = ColorsMap.Map[item.ColorID];

            var view = new CategoryItem
            {
                Name = item.Name,
                CircleColor = brush,
                IsSelected = item.ID == initialId,
                AutomationId = item.ID.ToString(),
            };

            view.Tapped += (s, e) => OnItemTapped(item.ID);
            ItemsContainer.Children.Add(view);
        }

        SelectedItemId = initialId;
    }

    private void OnItemTapped(long id)
    {
        SelectedItemId = id;
        SelectedItemChanged?.Invoke(this, new SelectedItemChangedEventArgs(id));
    }

    private void UpdateSelection(long? id)
    {
        if (id is null)
            return;

        foreach (var view in ItemsContainer.Children.OfType<CategoryItem>())
        {
            view.IsSelected = view.AutomationId == id.ToString();
        }
    }
}
