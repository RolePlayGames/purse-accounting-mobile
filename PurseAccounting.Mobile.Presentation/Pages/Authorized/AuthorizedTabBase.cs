using ReactiveUI;

namespace PurseAccountinng.Mobile.Presentation.Pages.Authorized;

public class AuthorizedTabBase : ReactiveObject
{
    public required string Header { get; init; }

    public required ContentView Tab { get; init; }
}
