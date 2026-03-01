using CommunityToolkit.Mvvm.Messaging;

namespace ChronoWiz;

public partial class SaveToICS : ContentPage
{
  private Ui.ZoomToWindowController? _zoomToWindow;

	public SaveToICS()
	{
		InitializeComponent();
		_zoomToWindow = new Ui.ZoomToWindowController(this, TotalStack);

		Summary.Focus();
	}

	protected override void OnDisappearing()
	{
		_zoomToWindow?.Dispose();
		_zoomToWindow = null;
		base.OnDisappearing();
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
		WeakReferenceMessenger.Default.Send
		(
			IcsDescription
			, MessengerKeys.SaveToIcsMessageKey
		);
	}
}