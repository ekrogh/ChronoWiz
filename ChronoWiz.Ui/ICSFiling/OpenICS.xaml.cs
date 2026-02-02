using CommunityToolkit.Mvvm.Messaging;
using ChronoWiz.Ui.MessageThings;

namespace ChronoWiz.Ui.ICSFiling;

public partial class OpenICS : ContentPage
{
	public OpenICS()
	{
		InitializeComponent();
	}

	private void Open_Button_Clicked(object sender, EventArgs e)
	{
		OpenIcsMessageArgs theArgs = new()
		{
			CorrectForTimeZone = SwitchTimeZone.IsToggled
		};

		WeakReferenceMessenger.Default.Send(theArgs, MessengerKeys.OpenIcsMessageKey);
	}
}
