namespace TimeCalculator;

public partial class FileICS : ContentPage
{
	public FileICS()
	{
		InitializeComponent();

		if (DeviceInfo.Platform == DevicePlatform.Android)
		{
			FileICSContentPageName.SetAppThemeColor(ContentPage.BackgroundColorProperty, Colors.White, Colors.Black);
			Resources["DynamicBaseButtonStyle"] = Resources["AndroidBaseButtonStyle"];
		}
		else
		{
			Resources["DynamicBaseButtonStyle"] = Resources["baseButtonStyle"];
		}

	}

	private async void OpenICSButton_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync
		(
			nameof(OpenICS)
			, true
		);
	}

	private async void SaveToICSButton_ClickedAsync(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync
		(
			nameof(SaveToICS)
			, true
		);
		await Navigation.PushAsync(new SaveToICS(), true);
	}
}