using CommunityToolkit.Mvvm.Messaging;

namespace TimeCalculator;

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
}