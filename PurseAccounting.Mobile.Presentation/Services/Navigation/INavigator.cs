namespace PurseAccountinng.Mobile.Presentation.Services.Navigation;

public interface INavigator
{
    /// <summary>
    /// Changes page to target
    /// </summary>
    /// <typeparam name="TContentPage">Target page</typeparam>
    Task ChangePageTo<TContentPage>(CancellationToken ct) where TContentPage : ContentPage;

    /// <summary>
    /// Changes active tab
    /// </summary>
    /// <typeparam name="TTab">Target tab</typeparam>
    Task ChangeTabTo<TTab>(CancellationToken ct) where TTab : ContentView;

    /// <summary>
    /// Activates authorized page with checks
    /// </summary>
    Task ActivateAuthorizedPage(CancellationToken ct);
}
