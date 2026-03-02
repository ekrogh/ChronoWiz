namespace ChronoWiz;

public partial class SettingsPage : ContentPage
{
	private bool _initialized;

	public SettingsPage()
	{
		InitializeComponent();

		if (!Ui.ZoomSettings.IsZoomSupportedForCurrentDevice)
		{
			ZoomSettingsSection.IsVisible = false;
			Ui.ZoomSettings.IsZoomToWindowEnabled = false;
			return;
		}

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
