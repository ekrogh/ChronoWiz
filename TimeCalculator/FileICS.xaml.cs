namespace TimeCalculator
{
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

				if
				(
						(DeviceInfo.Platform == DevicePlatform.macOS)
					|| (DeviceInfo.Platform == DevicePlatform.MacCatalyst))
				{
					FileCancelButton.IsVisible = true;
				}
			}

		}

		private async void OpenICSButton_Clicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new OpenICS(), true);
		}

		private async void SaveToICSButton_ClickedAsync(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new SaveToICS(), true);
		}

		private async void FileCancelButton_Clicked(object sender, EventArgs e)
		{
			await Navigation.PopAsync(true);
			//			await Navigation.PopToRootAsync(true);
		}
	}
}