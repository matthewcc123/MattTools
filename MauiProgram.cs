using MattTools.Services;
using Microsoft.Extensions.Logging;

namespace MattTools;

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
			});

		builder.Services.AddMauiBlazorWebView();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

        //My Services
        builder.Services.AddSingleton<MainLayoutService>();
        builder.Services.AddSingleton<ULIMergerService>();

        return builder.Build();
	}
}
