using CommunityToolkit.Maui;
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
            .UseMauiCommunityToolkit()
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
        builder.Services.AddSingleton<MainService>();
        builder.Services.AddSingleton<ULIMergerService>();
        builder.Services.AddSingleton<RossumExtractorService>();

        //HttpClient
        builder.Services.AddHttpClient<IRossumService, RossumService>( client =>
		{
			client.BaseAddress = new Uri("https://elis.app.rossum.ai/api/v1/");
		});

        return builder.Build();
	}
}
