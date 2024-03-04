namespace ChronoWiz;

public partial class AboutHelp : ContentPage
{
	public AboutHelp()
	{
		InitializeComponent();

		var curVer = AppInfo.Current.Version;

#if ANDROID || IOS
		string revsn = curVer.Revision > -1 ? curVer.Revision.ToString() : "";
#endif

		AppNameAndVer.Text =
							AppInfo.Current.Name
							+ "  Version: "
							+ curVer.Major
							+ '.'
							+ curVer.Minor
							+ '.'
							+ curVer.Build
#if ANDROID || IOS
							+ '.'
							+ revsn
#endif
							;
	}

	private async void UsersGuideButton_Clicked(object sender, EventArgs e)
	{
		if (await Launcher.CanOpenAsync(new Uri("http://eksit.dk/users-guide-2/")))
		{
			await Launcher.OpenAsync(new Uri("http://eksit.dk/users-guide-2/"));
		}
	}

	private async void MyUrlButton_Clicked(object sender, EventArgs e)
	{
		if (await Launcher.CanOpenAsync(new Uri("http://eksit.dk/")))
		{
			await Launcher.OpenAsync(new Uri("http://eksit.dk/"));
		}
	}

	private async void EmaiBtn_Clicked(object sender, EventArgs e)
	{
		if (await Launcher.CanOpenAsync(new Uri("mailto://timedatecalculator@eksit.dk")))
		{
			await Launcher.OpenAsync(new Uri("mailto://timedatecalculator@eksit.dk"));
		}
	}
}