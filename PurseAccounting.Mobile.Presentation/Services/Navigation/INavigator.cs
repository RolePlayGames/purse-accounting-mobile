namespace PurseAccountinng.Mobile.Presentation.Services.Navigation;

public interface INavigator
{
    Task ChangePageTo<TContentPage>(CancellationToken ct) where TContentPage : ContentPage;
}
