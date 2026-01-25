namespace PurseAccountinng.Mobile.Presentation.Components.Chips;

public class ChipsValueChangedEventArgs : EventArgs
{
    public object NewValue { get; }

    public ChipsValueChangedEventArgs(object newValue) => NewValue = newValue;
}
