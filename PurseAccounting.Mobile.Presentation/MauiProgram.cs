using CommunityToolkit.Maui;
using Material.Components.Maui.Extensions;
using MauiIcons.Material.Outlined;
using Microsoft.Extensions.Logging;
using PurseAccounting.Mobile.Application;
using PurseAccounting.Mobile.Presentation;

namespace PurseAccountinng.Mobile.Presentation
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseMaterialComponents()
                .UseMaterialOutlinedMauiIcons()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services
                .AddApplication()
                .AddPages()
                ;

            // Убираем подчёркивание у Entry
            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("NoUnderline", (handler, entry) =>
            {
#if ANDROID
                handler.PlatformView.Background = null;
#elif IOS
                handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#endif
            });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
