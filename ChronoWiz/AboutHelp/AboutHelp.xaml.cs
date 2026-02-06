using System.Runtime.Versioning;

namespace ChronoWiz;

public partial class AboutHelp : ContentPage
{
	public AboutHelp()
	{
		InitializeComponent();

		var name = AppInfo.Current.Name ?? "";
		var verStr = AppInfo.Current.VersionString ?? "";
		if (!System.Version.TryParse(verStr, out var curVer))
		{
			AppNameAndVer.Text = string.IsNullOrWhiteSpace(verStr)
				? name
				: name + "  Version: " + verStr;
			return;
		}

		AppNameAndVer.Text =
			name
			+ "  Version: "
			+ curVer.Major
			+ '.'
			+ curVer.Minor
			+ '.'
			+ curVer.Build;
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
		try
		{
			if (!Email.Default.IsComposeSupported)
			{
				await DisplayAlertAsync("Email", "Email is not supported on this device.", "OK");
				return;
			}

			var message = new EmailMessage
			{
				To = new List<string> { "eks@eksit.dk" },
				Subject = "ChronoWiz",
			};

			await Email.Default.ComposeAsync(message);
		}
		catch (Exception ex)
		{
			await DisplayAlertAsync("Email", ex.Message, "OK");
		}
	}
}