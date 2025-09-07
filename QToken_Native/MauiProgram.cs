using Microsoft.Maui.Platform;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Handlers;


namespace QToken_Native
{
    public static class MauiProgram
    {

        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            #region Handlers
            EntryHandler.Mapper.AppendToMapping("NoUnderline", (handler, view) =>
            {
#if ANDROID
    handler.PlatformView.Background = null; // removes native underline
#endif

#if IOS || MACCATALYST
    handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None; // removes iOS border
#endif

#if WINDOWS
    handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
#endif
            });

            PickerHandler.Mapper.AppendToMapping("NoUnderline", (handler, view) =>
            {
#if ANDROID
    handler.PlatformView.Background = null;
    handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
    handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Colors.Transparent.ToPlatform());
#endif

#if IOS || MACCATALYST
                handler.PlatformView.BackgroundColor = UIKit.UIColor.Clear;
    handler.PlatformView.Layer.BorderWidth = 0;
    handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#endif

#if WINDOWS
    handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
#endif
            });
            #endregion

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
