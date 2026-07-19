using PurseAccounting.Mobile.Infrastructure.TransactionCategories;
using PurseAccountinng.Mobile.Presentation.Colors;

namespace PurseAccountinng.Mobile.Presentation.Components.Categories;

public partial class MultiCategorySelector : ContentView
{
    public static readonly BindableProperty HeaderTextProperty
        = BindableProperty.Create(nameof(HeaderText), typeof(string), typeof(MultiCategorySelector), string.Empty);

    public static readonly BindableProperty ItemsSourceProperty
        = BindableProperty.Create(nameof(ItemsSource), typeof(IReadOnlyCollection<TransactionCategoryDto>), typeof(MultiCategorySelector), null, propertyChanged: OnItemsSourceChanged);

    public static readonly BindableProperty SelectedItemIdsProperty
        = BindableProperty.Create(nameof(SelectedItemIds), typeof(IReadOnlyCollection<long>), typeof(MultiCategorySelector), new HashSet<long>(0), BindingMode.TwoWay, propertyChanged: OnSelectedIdsChanged);

    public event EventHandler<SelectedItemsChangedEventArgs>? SelectedItemsChanged;

    public string HeaderText
    {
        get => (string)GetValue(HeaderTextProperty);
        set => SetValue(HeaderTextProperty, value);
    }

    public IReadOnlyCollection<TransactionCategoryDto> ItemsSource
    {
        get => (IReadOnlyCollection<TransactionCategoryDto>)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public IReadOnlyCollection<long> SelectedItemIds
    {
        get => (IReadOnlyCollection<long>)GetValue(SelectedItemIdsProperty);
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
            control.UpdateSelection((IReadOnlyCollection<long>?)newValue);
    }

    private void UpdateItems()
    {
        foreach (var view in ItemsContainer.Children.OfType<CategoryItem>())
        {
            if (long.TryParse(view.AutomationId, out var itemId))
            {
                view.Tapped -= (s, e) => OnItemTapped(itemId);
            }
        }

        ItemsContainer.Children.Clear();

        if (ItemsSource is null || ItemsSource.Count == 0)
            return;

        var newIds = SelectedItemIds?.ToHashSet() ?? [];

        foreach (var item in ItemsSource)
        {
            var brush = ColorsMap.Map[item.ColorID];

            var view = new CategoryItem
            {
                Name = item.Name,
                CircleColor = brush,
                IsSelected = newIds.Contains(item.ID),
                AutomationId = item.ID.ToString(),
            };

            view.Tapped += (s, e) => OnItemTapped(item.ID);
            ItemsContainer.Children.Add(view);
        }

        SelectedItemIds = newIds;
    }

    private void OnItemTapped(long id)
    {
        var newIds = SelectedItemIds?.ToHashSet() ?? new HashSet<long>(1);

        if (!newIds.Remove(id))
            newIds.Add(id);

        SelectedItemIds = newIds;

        SelectedItemsChanged?.Invoke(this, new SelectedItemsChangedEventArgs(SelectedItemIds));
    }

    private void UpdateSelection(IReadOnlyCollection<long>? ids)
    {
        if (ids is null)
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
