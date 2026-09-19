using System.Windows.Input;

namespace PurseAccountinng.Mobile.Presentation.Components;

public partial class AddScheduledTransactionButton : ContentView
{
    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(
            nameof(Command),
            typeof(ICommand),
            typeof(AddScheduledTransactionButton));

    public ICommand? Command
    {
        get => (ICommand?)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public AddScheduledTransactionButton()
    {
        InitializeComponent();
    }
}
