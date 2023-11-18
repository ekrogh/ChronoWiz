namespace DateAndTimeCalculator
{
	public static class MauiProgram
	{
		public static MauiApp CreateMauiApp()
		{
			var builder = MauiApp.CreateBuilder();
			builder
				.UseMauiCommunityToolkit()
				.UseMauiApp<App>()
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("opensansregular.ttf", "OpenSansRegular");
					fonts.AddFont("opensanssemibold.ttf", "OpenSansSemibold");
				});
#if DEBUG
			builder.Logging.AddDebug();
#endif
			//builder.Services.AddSingleton<IFileSaver>(FileSaver.Default);

			return builder.Build();
		}
	}
}