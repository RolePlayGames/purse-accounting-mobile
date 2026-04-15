namespace PurseAccountinng.Mobile.Presentation.Components.Categories;

public class SelectedItemsChangedEventArgs : EventArgs
{
    public IReadOnlyCollection<long> SelectedIds { get; }

    public SelectedItemsChangedEventArgs(IReadOnlyCollection<long> ids) => SelectedIds = ids;
}
