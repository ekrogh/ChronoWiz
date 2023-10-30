using CommunityToolkit.Mvvm.Messaging;

namespace TimeCalculator;

public partial class SaveToICS : ContentPage
{
	public SaveToICS()
	{
		InitializeComponent();

		Summary.Focus();
	}

	//#if WINDOWS
#if (WINDOWS || ANDROID)
	protected override void OnSizeAllocated(double width, double height)
	{
		base.OnSizeAllocated(width, height);

		TotalStack.Scale = 1.0f / TotalStack.Scale;

		var DisplayOrientation = DeviceDisplay.Current.MainDisplayInfo.Orientation;

		double ScaleF = 1.0f;
		if (DisplayOrientation == DisplayOrientation.Landscape)
		{
			ScaleF = 0.9f;
		}

		double WidthFactor = width * ScaleF / TotalStack.Width;
		double HeightFactor = height * ScaleF / TotalStack.Height;

		if (WidthFactor < HeightFactor)
		{
			TotalStack.Scale = WidthFactor;
		}
		else
		{
			TotalStack.Scale = HeightFactor;
		}

		Dispatcher.Dispatch(() =>
			(SaveToICSContentPageName as IView).InvalidateArrange());

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