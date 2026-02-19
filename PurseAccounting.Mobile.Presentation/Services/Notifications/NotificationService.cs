using PurseAccountinng.Mobile.Presentation.Components;

namespace PurseAccountinng.Mobile.Presentation.Services.Notifications;

internal class NotificationService : INotificationService
{
    private static readonly Color SuccessColor = Color.FromArgb("#4CAF50");
    private static readonly Color ErrorColor = Color.FromArgb("#F44336");

    public void ShowSuccess(string message) => Show(message, SuccessColor);

    public void ShowError(string message) => Show(message, ErrorColor);

    private static void Show(string message, Color color)
    {
        MainThread.InvokeOnMainThreadAsync(async () =>
        {
            var toast = new ToastView
            {
                Text = message,
                BackgroundColor = color,
            };
            await toast.ShowAsync();
        });
    }
}
