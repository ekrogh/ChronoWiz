namespace ChronoWiz;

public partial class FileICS : ContentPage
{
#if WINDOWS
	private double _lastWidth = -1;
	private double _lastHeight = -1;
	private bool _isScaling;
#endif

	public FileICS()
	{
		InitializeComponent();
	}

#if WINDOWS
	protected override void OnSizeAllocated(double width, double height)
	{
		base.OnSizeAllocated(width, height);

		if (_isScaling) return;
		if (Math.Abs(_lastWidth - width) < double.Epsilon &&
		    Math.Abs(_lastHeight - height) < double.Epsilon) return;

		_lastWidth = width;
		_lastHeight = height;

		if (width <= 0 || height <= 0 || TotalStack.Width <= 0 || TotalStack.Height <= 0) return;

		double widthFactor = width / TotalStack.Width;
		double heightFactor = height / TotalStack.Height;
		double newScale = Math.Min(widthFactor, heightFactor);
		if (newScale <= 0) return;

		if (Math.Abs(TotalStack.Scale - newScale) > 0.001)
		{
			_isScaling = true;
			try { TotalStack.Scale = newScale; }
			finally { _isScaling = false; }
		}
	}
#endif

	private async void OpenICSButton_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(OpenICS), true);
	}

	private async void SaveToICSButton_ClickedAsync(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(SaveToICS), true);
	}
}