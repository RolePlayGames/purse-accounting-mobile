using PurseAccountinng.Mobile.Presentation.Pages;

namespace PurseAccountinng.Mobile.Presentation;

/// <summary>
/// Main app class
/// </summary>
public partial class App : Application
{
    private readonly IServiceProvider _serviceProvider;

    public App(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var logoPage = _serviceProvider.GetRequiredService<LogoPage>();
        return new Window(logoPage);
    }
}
