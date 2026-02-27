using PurseAccountinng.Mobile.Presentation.Components;

namespace PurseAccountinng.Mobile.Presentation.Services.Notifications;

internal class NotificationService : INotificationService
{
    private static readonly Color SuccessColor = Color.FromArgb("#4CAF50");
    private static readonly Color ErrorColor = Color.FromArgb("#F44336");

    public void ShowSuccess(string message, double bottomMargin = 0) => Show(message, bottomMargin, SuccessColor);

    public void ShowError(string message, double bottomMargin = 0) => Show(message, bottomMargin, ErrorColor);

    private static void Show(string message, double bottomMargin, Color color)
    {
        MainThread.InvokeOnMainThreadAsync(async () =>
        {
            var toast = new ToastView
            {
                Text = message,
                BackgroundColor = color,
            };
            await toast.ShowAsync(bottomMargin);
        });
    }
}
