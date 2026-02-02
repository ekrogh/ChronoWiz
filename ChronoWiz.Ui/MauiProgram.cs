namespace ChronoWiz.Ui;

using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();

#if (ANDROID || TIZEN || WINDOWS) || (IOS && __IOS_15__) || (MACCATALYST && __MACCATALYST_15__)
		builder.UseMauiCommunityToolkit();
#endif

		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("opensansregular.ttf", "OpenSansRegular");
				fonts.AddFont("opensanssemibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
