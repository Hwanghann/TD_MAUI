using Microsoft.Extensions.Logging;
using MauiApp1.Services;
using MauiApp1.ViewModels;

namespace MauiApp1;

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

        // Services TD3
        builder.Services.AddSingleton<IGameHistoryService, FakeGameHistoryService>();
        builder.Services.AddSingleton<IBotService, RandomBotService>();
        builder.Services.AddSingleton<IGameEngine, GameEngine>();

        // VM
        builder.Services.AddTransient<GameViewModel>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}