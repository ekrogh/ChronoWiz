namespace ChronoWiz;

public partial class SettingsPage : ContentPage
{
	private bool _initialized;

	public SettingsPage()
	{
		InitializeComponent();
		ZoomToWindowSwitch.IsToggled = Ui.ZoomSettings.IsZoomToWindowEnabled;
		_initialized = true;
	}

	private void ZoomToWindowSwitch_Toggled(object sender, ToggledEventArgs e)
	{
		if (!_initialized)
			return;

		Ui.ZoomSettings.IsZoomToWindowEnabled = e.Value;
	}
}
