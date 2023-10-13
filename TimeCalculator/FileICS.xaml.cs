namespace TimeCalculator
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FileICS : ContentPage
	{
		public FileICS()
		{
			InitializeComponent();

			// TODO Xamarin.Forms.Device.RuntimePlatform is no longer supported. Use Microsoft.Maui.Devices.DeviceInfo.Platform instead. For more details see https://learn.microsoft.com/en-us/dotnet/maui/migration/forms-projects#device-changes
			if (DeviceInfo.Platform == DevicePlatform.Android)
			{
				FileICSContentPageName.SetAppThemeColor(ContentPage.BackgroundColorProperty, Colors.White, Colors.Black);
				Resources["DynamicBaseButtonStyle"] = Resources["AndroidBaseButtonStyle"];
			}
			else
			{
				Resources["DynamicBaseButtonStyle"] = Resources["baseButtonStyle"];

				if (DeviceInfo.Platform == DevicePlatform.macOS)
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