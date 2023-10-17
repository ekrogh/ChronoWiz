using TimeCalculator.MessageThings;
using Microsoft.Maui.Graphics;
using CommunityToolkit.Mvvm.Messaging;

namespace TimeCalculator
{
	public partial class OpenICS : ContentPage
	{

		public OpenICS()
		{
			InitializeComponent();

			if (DeviceInfo.Platform == DevicePlatform.Android)
			{
				OpenICSContentPageName.SetAppThemeColor(ContentPage.BackgroundColorProperty, Colors.White, Colors.Black);
				Resources["DynamicBaseButtonStyle"] = Resources["AndroidBaseButtonStyle"];
			}
			else
			{
				Resources["DynamicBaseButtonStyle"] = Resources["baseButtonStyle"];

				if (DeviceInfo.Platform == DevicePlatform.MacCatalyst)
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
			WeakReferenceMessenger.Default.Send
			(
				TheAgr
				, MessengerKeys.OpenIcsMessageKey
			);

		}

		private async void FileOpenCancelButtn_Clicked(object sender, System.EventArgs e)
		{
			await Navigation.PopAsync(true);
			//			await Navigation.PopToRootAsync(true);
		}
	}
}