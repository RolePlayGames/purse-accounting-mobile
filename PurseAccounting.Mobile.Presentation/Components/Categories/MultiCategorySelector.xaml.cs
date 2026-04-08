using PurseAccounting.Mobile.Infrastructure.TransactionCategories;
using PurseAccountinng.Mobile.Presentation.Colors;

namespace PurseAccountinng.Mobile.Presentation.Components.Categories;

public partial class MultiCategorySelector : ContentView
{
    public static readonly BindableProperty HeaderTextProperty
        = BindableProperty.Create(nameof(HeaderText), typeof(string), typeof(MultiCategorySelector), string.Empty);

    public static readonly BindableProperty ItemsSourceProperty
        = BindableProperty.Create(nameof(ItemsSource), typeof(IList<TransactionCategoryDto>), typeof(MultiCategorySelector), null, propertyChanged: OnItemsSourceChanged);

    public static readonly BindableProperty SelectedItemIdsProperty
        = BindableProperty.Create(nameof(SelectedItemIds), typeof(IList<long>), typeof(MultiCategorySelector), null, BindingMode.TwoWay, propertyChanged: OnSelectedIdsChanged);

    public event EventHandler<SelectedItemsChangedEventArgs>? SelectedItemsChanged;

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

    public IList<long> SelectedItemIds
    {
        get => (IList<long>)GetValue(SelectedItemIdsProperty);
        set => SetValue(SelectedItemIdsProperty, value);
    }

    public MultiCategorySelector()
    {
        InitializeComponent();
    }

    private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is MultiCategorySelector control)
            control.UpdateItems();
    }

    private static void OnSelectedIdsChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is MultiCategorySelector control && control.ItemsSource != null)
        {
            control.UpdateSelection((IList<long>?)newValue);
        }
    }

    private void UpdateItems()
    {
        ItemsContainer.Children.Clear();

        if (ItemsSource == null || ItemsSource.Count == 0)
            return;

        var initialIds = SelectedItemIds ?? new List<long>();
        
        // Если ничего не выбрано, выбираем категорию по умолчанию
        if (initialIds.Count == 0)
        {
            var defaultCategory = ItemsSource.FirstOrDefault(x => x.IsDefault) ?? ItemsSource.First();
            initialIds = new List<long> { defaultCategory.ID };
        }

        foreach (var item in ItemsSource)
        {
            var brush = ColorsMap.Map[item.ColorID];

            var view = new CategoryItem
            {
                Name = item.Name,
                CircleColor = brush,
                IsSelected = initialIds.Contains(item.ID),
                AutomationId = item.ID.ToString(),
            };

            view.Tapped += (s, e) => OnItemTapped(item.ID);
            ItemsContainer.Children.Add(view);
        }

        SelectedItemIds = initialIds;
    }

    private void OnItemTapped(long id)
    {
        if (SelectedItemIds == null)
            SelectedItemIds = new List<long>();

        if (SelectedItemIds.Contains(id))
        {
            // Если уже выбрано и это последний элемент, не снимаем выделение (оставляем хотя бы один выбранный)
            if (SelectedItemIds.Count > 1)
            {
                SelectedItemIds.Remove(id);
            }
        }
        else
        {
            SelectedItemIds.Add(id);
        }

        SelectedItemsChanged?.Invoke(this, new SelectedItemsChangedEventArgs(SelectedItemIds));
    }

    private void UpdateSelection(IList<long>? ids)
    {
        if (ids == null)
            return;

        foreach (var view in ItemsContainer.Children.OfType<CategoryItem>())
        {
            if (long.TryParse(view.AutomationId, out var itemId))
            {
                view.IsSelected = ids.Contains(itemId);
            }
        }
    }
}

