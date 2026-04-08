namespace PurseAccountinng.Mobile.Presentation.Components.Categories;

public class SelectedItemsChangedEventArgs : EventArgs
{
    public IList<long> SelectedIds { get; }

    public SelectedItemsChangedEventArgs(IList<long> ids) => SelectedIds = ids;
}

