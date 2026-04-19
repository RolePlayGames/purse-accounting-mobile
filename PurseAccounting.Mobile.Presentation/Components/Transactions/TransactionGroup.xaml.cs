using System.Collections;
using System.Collections.Specialized;
using System.Windows.Input;

namespace PurseAccountinng.Mobile.Presentation.Components.Transactions;

public partial class TransactionGroup : ContentView
{
    public static readonly BindableProperty ItemsProperty =
        BindableProperty.Create(nameof(Items), typeof(IList), typeof(TransactionGroup), null,
            propertyChanged: OnItemsChanged);

    public static readonly BindableProperty ItemTemplateProperty =
        BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(TransactionGroup), null,
            propertyChanged: OnItemTemplateChanged);

    public static readonly BindableProperty ItemSwipedCommandProperty =
        BindableProperty.Create(nameof(ItemSwipedCommand), typeof(ICommand), typeof(TransactionGroup), null);

    private IList? _currentItems;

    public IList? Items
    {
        get => (IList?)GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }

    public DataTemplate? ItemTemplate
    {
        get => (DataTemplate?)GetValue(ItemTemplateProperty);
        set => SetValue(ItemTemplateProperty, value);
    }

    public ICommand? ItemSwipedCommand
    {
        get => (ICommand?)GetValue(ItemSwipedCommandProperty);
        set => SetValue(ItemSwipedCommandProperty, value);
    }

    public TransactionGroup()
    {
        InitializeComponent();
        UpdateItemsSource();
    }

    private static void OnItemsChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TransactionGroup group)
        {
            group.OnItemsChanged();
        }
    }

    private static void OnItemTemplateChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TransactionGroup group)
        {
            group.UpdateItemsSource();
        }
    }

    private void OnItemsChanged()
    {
        if (_currentItems is INotifyCollectionChanged oldCollection)
        {
            oldCollection.CollectionChanged -= OnCollectionChanged;
        }

        _currentItems = Items;

        if (_currentItems is INotifyCollectionChanged newCollection)
        {
            newCollection.CollectionChanged += OnCollectionChanged;
        }

        UpdateItemsSource();
    }

    private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        UpdateItemsSource();
    }

    private void UpdateItemsSource()
    {
        ItemsStackLayout.Children.Clear();

        if (Items == null || ItemTemplate == null)
        {
            return;
        }

        foreach (var item in Items)
        {
            var content = ItemTemplate.CreateContent() as View;
            if (content != null)
            {
                var transactionRow = new TransactionRow
                {
                    Content = content,
                    BindingContext = item
                };
                transactionRow.Swiped += OnTransactionRowSwiped;
                ItemsStackLayout.Children.Add(transactionRow);
            }
        }
    }

    private void OnTransactionRowSwiped(object? sender, EventArgs e)
    {
        if (sender is TransactionRow row && ItemSwipedCommand?.CanExecute(row.BindingContext) == true)
        {
            ItemSwipedCommand.Execute(row.BindingContext);
        }
    }
}
