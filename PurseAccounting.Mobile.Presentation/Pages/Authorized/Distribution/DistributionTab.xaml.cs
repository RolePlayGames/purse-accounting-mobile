namespace PurseAccountinng.Mobile.Presentation.Pages.Authorized.Distribution;

public partial class DistributionTab : ContentView
{
    public DistributionTab(DistributionTabViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }
}
