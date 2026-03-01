namespace ChronoWiz;

public partial class FileICS : ContentPage
{
    private Ui.ZoomToWindowController? _zoomToWindow;

	public FileICS()
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

	private async void OpenICSButton_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(OpenICS), true);
	}

	private async void SaveToICSButton_ClickedAsync(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(SaveToICS), true);
	}
}