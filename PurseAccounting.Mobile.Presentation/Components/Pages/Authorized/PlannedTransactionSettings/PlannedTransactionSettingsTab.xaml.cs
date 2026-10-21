namespace PurseAccountinng.Mobile.Presentation.Components.Pages.Authorized.PlannedTransactionSettings;

public partial class PlannedTransactionSettingsTab : ContentView
{
    public PlannedTransactionSettingsTab(
        PlannedTransactionSettingsTabViewModel viewModel,
        PlannedTransactionAttributesWidgetViewModel widgetViewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
        PlannedTransactionAttributesWidget.BindingContext = widgetViewModel;
    }
}
