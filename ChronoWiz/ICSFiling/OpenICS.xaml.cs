using CommunityToolkit.Mvvm.Messaging;

namespace ChronoWiz;

public partial class OpenICS : ContentPage
{
    private Ui.ZoomToWindowController? _zoomToWindow;

	public OpenICS()
	{
		InitializeComponent();
       _zoomToWindow = new Ui.ZoomToWindowController(this, TotalStack);
	}

	protected override void OnDisappearing()
	{
		_zoomToWindow?.Dispose();
		_zoomToWindow = null;
		base.OnDisappearing();
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