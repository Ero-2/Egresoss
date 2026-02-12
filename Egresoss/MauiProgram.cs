using Microsoft.Extensions.Logging;
using Egresoss.Services;
using Egresoss.ViewModels;
using Microcharts.Maui;

namespace Egresoss
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMicrocharts() // 👈 AGREGADO
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<DatabaseService>();
            builder.Services.AddTransient<MainViewModel>();
            builder.Services.AddTransient<TransactionViewModel>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<NewTransactionPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}