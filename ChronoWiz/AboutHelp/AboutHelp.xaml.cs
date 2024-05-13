using Microsoft.Maui.ApplicationModel;
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
		_ = await Browser.Default.OpenAsync
				(new Uri("http://eksit.dk/users-guide-3/"), BrowserLaunchMode.SystemPreferred);
	}

	private async void MyUrlButton_Clicked(object sender, EventArgs e)
	{
		_ = await Browser.Default.OpenAsync
			(new Uri("http://eksit.dk/"), BrowserLaunchMode.SystemPreferred);
	}

	private async void EmaiBtn_Clicked(object sender, EventArgs e)
	{
		_ = await Browser.Default.OpenAsync
			(new Uri("mailto:eks@eksit.dk"), BrowserLaunchMode.SystemPreferred);
	}
}