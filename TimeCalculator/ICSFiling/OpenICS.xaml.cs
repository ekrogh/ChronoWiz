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