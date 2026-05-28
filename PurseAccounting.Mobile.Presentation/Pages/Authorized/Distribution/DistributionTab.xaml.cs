using PurseAccounting.Mobile.Application.Distribution;

namespace PurseAccountinng.Mobile.Presentation.Pages.Authorized.Distribution;

public partial class DistributionTab : ContentView
{
    public DistributionTab(IServiceProvider serviceProvider, AvailableUserChoiceDistributionStrategyInfo? availableUserChoiceDistributionStrategy = null)
    {
        InitializeComponent();

        var distributionTabViewModel = ActivatorUtilities.CreateInstance<DistributionTabViewModel>(serviceProvider, availableUserChoiceDistributionStrategy);
        BindingContext = distributionTabViewModel;
    }
}
