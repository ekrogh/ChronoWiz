using CommunityToolkit.Mvvm.Messaging;

namespace ChronoWiz;

public partial class OpenICS : ContentPage
{

	public OpenICS()
	{
		InitializeComponent();
	}

#if WINDOWS
	private double _lastWidth = -1;
	private double _lastHeight = -1;
	private bool _isScaling;
	protected override void OnSizeAllocated(double width, double height)
	{
		base.OnSizeAllocated(width, height);

		if (_isScaling)
			return;
		if (Math.Abs(_lastWidth - width) < double.Epsilon && Math.Abs(_lastHeight - height) < double.Epsilon)
			return;
		_lastWidth = width;
		_lastHeight = height;

		if (width <= 0 || height <= 0 || TotalStack.Width <= 0 || TotalStack.Height <= 0)
			return;

		double widthFactor = width / TotalStack.Width;
		double heightFactor = height / TotalStack.Height;
		double newScale = widthFactor < heightFactor ? widthFactor : heightFactor;
		if (newScale <= 0)
			return;

		if (Math.Abs(TotalStack.Scale - newScale) > 0.001)
		{
			_isScaling = true;
			try
			{
				TotalStack.Scale = newScale;
			}
			finally
			{
				_isScaling = false;
			}
		}
	}
#endif

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