namespace PurseAccountinng.Mobile.Presentation.Pages.Authorized.Distribution;

public partial class DistributionTab : ContentView
{
    public DistributionTab(IServiceProvider serviceProvider)
    {
        InitializeComponent();

        var distributionTabViewModel = ActivatorUtilities.CreateInstance<DistributionTabViewModel>(serviceProvider);
        BindingContext = distributionTabViewModel;
    }
}
