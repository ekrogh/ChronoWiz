using CommunityToolkit.Mvvm.Messaging;

namespace ChronoWiz;

public partial class SaveToICS : ContentPage
{
	public SaveToICS()
	{
		InitializeComponent();

		Summary.Focus();
	}

	// Track last allocated size and re-entrancy to avoid layout thrashing
	private double _lastAllocatedHeight = -1;
	private bool _isUpdatingScale;

#if (WINDOWS || ANDROID)
	// One-shot handler to apply scale when content is actually measured
	private void TotalStack_SizeChanged_ApplyScaleOnce(object? sender, EventArgs e)
	{
		TotalStack.SizeChanged -= TotalStack_SizeChanged_ApplyScaleOnce;
		ApplyScale(Width, Height);
		_lastAllocatedHeight = Height;
	}

	private void ApplyScale(double width, double height)
	{
		if (_isUpdatingScale)
			return;
		if (width <= 0 || height <= 0)
			return;
		if (TotalStack.Width <= 0 || TotalStack.Height <= 0)
			return;

		var orientation = DeviceDisplay.Current.MainDisplayInfo.Orientation;
		double scaleF = orientation == DisplayOrientation.Landscape ? 0.9 : 1.0;

		double widthFactor = width * scaleF / TotalStack.Width;
		double heightFactor = height * scaleF / TotalStack.Height;
		double newScale = widthFactor < heightFactor ? widthFactor : heightFactor;
		if (newScale <= 0)
			return;

		if (Math.Abs(TotalStack.Scale - newScale) > 0.001)
		{
			_isUpdatingScale = true;
			try
			{
				TotalStack.Scale = newScale;
			}
			finally
			{
				_isUpdatingScale = false;
			}
		}
	}

	protected override void OnSizeAllocated(double width, double height)
	{
		base.OnSizeAllocated(width, height);

		// Ignore duplicate allocations
		// Only check _lastAllocatedHeight now
		if (Math.Abs(_lastAllocatedHeight - height) < double.Epsilon)
		{
			// If initial scale hasn’t run yet but sizes are valid, apply it once
			if (TotalStack.Width > 0 && TotalStack.Height > 0)
			{
				ApplyScale(width, height);
			}
			return;
		}

		// If content isn't measured yet, apply scale once when it is
		if (TotalStack.Width <= 0 || TotalStack.Height <= 0)
		{
			TotalStack.SizeChanged -= TotalStack_SizeChanged_ApplyScaleOnce;
			TotalStack.SizeChanged += TotalStack_SizeChanged_ApplyScaleOnce;
			return;
		}

		// Normal path
		ApplyScale(width, height);
		_lastAllocatedHeight = height;
	}
#endif

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