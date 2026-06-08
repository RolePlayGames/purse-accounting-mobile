using PurseAccounting.Mobile.Application.Context;
using PurseAccountinng.Mobile.Presentation.Pages.Authorized;

namespace PurseAccountinng.Mobile.Presentation.Services.Tabs;

public interface ITabNavigator
{
    /// <summary>
    /// Changed active tab to target on authorized page
    /// </summary>
    /// <typeparam name="TTab">Target tab</typeparam>
    void ChangeTabTo<TTab>() where TTab : ContentView;

    /// <summary>
    /// Inizialize tabs collection
    /// </summary>
    Task InitializeTabs();

    /// <summary>
    /// Active tab
    /// </summary>
    AuthorizedTabBase? ActiveTab { get; }

    /// <summary>
    /// On active tab changed
    /// </summary>
    event ValueChangedEventHandler<AuthorizedTabBase> ActiveTabChanged;
}
