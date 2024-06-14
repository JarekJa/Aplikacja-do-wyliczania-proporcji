using Aplikacja_do_wyliczania_proporcji.Sercices;
using Aplikacja_do_wyliczania_proporcji.Sercices.Interfaces;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using Aplikacja_do_wyliczania_proporcji.Views;
using Plugin.MauiMTAdmob;

namespace Aplikacja_do_wyliczania_proporcji
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>()
                .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            }).UseMauiCommunityToolkit()
              .UseMauiMTAdmob();
            builder.Services.AddSingleton<IDataBase, DataBase>();
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<ListsPage>();
#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}