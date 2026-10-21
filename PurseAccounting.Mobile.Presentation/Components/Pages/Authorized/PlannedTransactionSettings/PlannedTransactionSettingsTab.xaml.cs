using PurseAccountinng.Mobile.Presentation.Components.PlannedTransactions;

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

    private void OnAutoPlannedTransactionSwiped(object? sender, AutoPlannedTransactionSwipedEventArgs e)
    {
        if (BindingContext is PlannedTransactionSettingsTabViewModel viewModel)
        {
            _ = viewModel.DeleteAutoPlannedTransactionAsync(e.PlannedTransactionSettingInfo);
        }
    }
}
