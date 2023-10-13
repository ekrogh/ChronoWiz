using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Storage;
using Microsoft.Extensions.Logging;

namespace TimeCalculator;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			// Initialize the .NET MAUI Community Toolkit by adding the below line of code
			.UseMauiCommunityToolkit()
			.UseMauiCommunityToolkitCore()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			}).ConfigureEssentials(essentials =>
			{
				essentials.UseVersionTracking();
			});
		;

#if DEBUG
		builder.Logging.AddDebug();
#endif

		builder.Services.AddSingleton<IFileSaver>(FileSaver.Default);

		return builder.Build();
	}
}