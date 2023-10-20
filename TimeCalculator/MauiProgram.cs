using CommunityToolkit.Maui.Markup;
using Microsoft.Extensions.Logging;

namespace TimeCalculator
{
	public static class MauiProgram
	{
		public static MauiApp CreateMauiApp()
		{
			var builder = MauiApp.CreateBuilder();
			builder
				.UseMauiCommunityToolkit()
				.UseMauiCommunityToolkitMarkup()
				.UseMauiApp<App>()
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
					fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				});
#if DEBUG
			builder.Logging.AddDebug();
#endif
			//builder.Services.AddSingleton<IFileSaver>(FileSaver.Default);

			return builder.Build();
		}
	}
}