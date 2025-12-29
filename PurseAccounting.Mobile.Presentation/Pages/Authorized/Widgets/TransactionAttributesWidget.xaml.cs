using PurseAccountinng.Mobile.Presentation.Components.Chips;

namespace PurseAccountinng.Mobile.Presentation.Pages.Authorized.Widgets;

public partial class TransactionAttributesWidget : ContentView
{
	public TransactionAttributesWidget()
	{
		InitializeComponent();
    }

    void OnOptionChanged(object sender, ChipsValueChangedEventArgs e)
    {
        var selected = "Val";
    }
}
