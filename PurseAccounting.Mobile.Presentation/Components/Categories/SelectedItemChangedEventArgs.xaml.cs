namespace PurseAccountinng.Mobile.Presentation.Components.Categories;

public class SelectedItemChangedEventArgs : EventArgs
{
    public long SelectedId { get; }

    public SelectedItemChangedEventArgs(long id) => SelectedId = id;
}
