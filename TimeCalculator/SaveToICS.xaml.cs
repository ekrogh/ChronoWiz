using TimeDateCalculator;
using TimeDateCalculator.MessageThings;

namespace TimeDateCalculatorDll
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SaveToICS : ContentPage
	{
		public SaveToICS()
		{
			InitializeComponent();

			// TODO Xamarin.Forms.Device.RuntimePlatform is no longer supported. Use Microsoft.Maui.Devices.DeviceInfo.Platform instead. For more details see https://learn.microsoft.com/en-us/dotnet/maui/migration/forms-projects#device-changes
			if (DeviceInfo.Platform == DevicePlatform.Android)
			{
				SaveToICSContentPageName.SetAppThemeColor(ContentPage.BackgroundColorProperty, Colors.White, Colors.Black);
				Resources["DynamicBaseButtonStyle"] = Resources["AndroidBaseButtonStyle"];
			}
			else
			{
				Resources["DynamicBaseButtonStyle"] = Resources["baseButtonStyle"];

				if (DeviceInfo.Platform == DevicePlatform.macOS)
				{
					FileSaveToCancelButton.IsVisible = true;
				}
			}

			Summary.Focus();
		}

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
		private async void SaveICSButton_Clicked(object sender, EventArgs e)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
		{

			SaveToIcsMessageArgs IcsDescription = new SaveToIcsMessageArgs
			{
				EventName_Summary = Summary.Text,
				TheDescription = Description.Text,
				Location = LocationEntry.Text
			};

			if ((Summary.Text == null) || (Summary.Text == ""))
			{
				IcsDescription.EventName_Summary = "Summary";
			}
			if ((Description.Text == null) || (Description.Text == ""))
			{
				IcsDescription.TheDescription = "Description";
			}
			if ((LocationEntry.Text == null) || (LocationEntry.Text == ""))
			{
				IcsDescription.Location = "Location";
			}

			// Fire the message
			MessagingCenter.Send
				(
					(App)Application.Current,
					MessengerKeys.SaveToIcsMessageKey,
					IcsDescription
				);


		}

		private async void FileSaveToCancelButton_Clicked(object sender, EventArgs e)
		{
			await Navigation.PopAsync(true);
			//			await Navigation.PopToRootAsync(true);
		}
	}
}