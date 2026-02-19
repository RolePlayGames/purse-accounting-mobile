namespace PurseAccountinng.Mobile.Presentation.Services.Notifications;

public interface INotificationService
{
    void ShowSuccess(string message);

    void ShowError(string message);
}
