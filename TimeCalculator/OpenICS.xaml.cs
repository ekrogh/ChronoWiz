using TimeDateCalculator;
using TimeDateCalculator.MessageThings;

namespace TimeDateCalculatorDll
{
	public partial class OpenICS : ContentPage
	{

		public OpenICS()
		{
			InitializeComponent();

			// TODO Xamarin.Forms.Device.RuntimePlatform is no longer supported. Use Microsoft.Maui.Devices.DeviceInfo.Platform instead. For more details see https://learn.microsoft.com/en-us/dotnet/maui/migration/forms-projects#device-changes
			if (DeviceInfo.Platform == DevicePlatform.Android)
			{
				OpenICSContentPageName.SetAppThemeColor(ContentPage.BackgroundColorProperty, Colors.White, Colors.Black);
				Resources["DynamicBaseButtonStyle"] = Resources["AndroidBaseButtonStyle"];
			}
			else
			{
				Resources["DynamicBaseButtonStyle"] = Resources["baseButtonStyle"];

				if (DeviceInfo.Platform == DevicePlatform.macOS)
				{
					FileOpenCancelButton.IsVisible = true;
				}
			}
		}

		private void Open_Button_Clicked(object sender, System.EventArgs e)
		{
			OpenIcsMessageArgs TheAgr = new OpenIcsMessageArgs
			{
				CorrectForTimeZone = SwitchTimeZone.IsToggled
			};

			// Fire the message
			MessagingCenter.Send
					(
						(App)Application.Current,
						MessengerKeys.OpenIcsMessageKey,
						TheAgr
					);

		}

		private async void FileOpenCancelButtn_Clicked(object sender, System.EventArgs e)
		{
			await Navigation.PopAsync(true);
			//			await Navigation.PopToRootAsync(true);
		}
	}
}