using CommunityToolkit.Mvvm.Messaging;
using ChronoWiz.Ui.MessageThings;

namespace ChronoWiz.Ui.ICSFiling;

public partial class SaveToICS : ContentPage
{
	public SaveToICS()
	{
		InitializeComponent();
		Summary.Focus();
	}

#pragma warning disable CS1998
	private async void SaveICSButton_Clicked(object sender, EventArgs e)
#pragma warning restore CS1998
	{
		SaveToIcsMessageArgs args = new()
		{
			EventName_Summary = Summary.Text,
			TheDescription = Description.Text,
			Location = LocationEntry.Text
		};

		if (string.IsNullOrEmpty(Summary.Text))		args.EventName_Summary = "Summary";
		if (string.IsNullOrEmpty(Description.Text))	args.TheDescription = "Description";
		if (string.IsNullOrEmpty(LocationEntry.Text)) args.Location = "Location";

		WeakReferenceMessenger.Default.Send(args, MessengerKeys.SaveToIcsMessageKey);
	}
}
