namespace PurseAccountinng.Mobile.Presentation.Services.Notifications;

public interface INotificationService
{
    /// <summary>
    /// Shows success notification
    /// </summary>
    /// <param name="message">Notification's text</param>
    /// <param name="bottomMargin">Toast margin from bottom</param>
    void ShowSuccess(string message, double bottomMargin = 0);

    /// <summary>
    /// Shows error notification
    /// </summary>
    /// <param name="message">Notification's text</param>
    /// <param name="bottomMargin">Toast margin from bottom</param>
    void ShowError(string message, double bottomMargin = 0);
}
