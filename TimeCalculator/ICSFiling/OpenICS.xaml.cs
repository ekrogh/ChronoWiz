using CommunityToolkit.Mvvm.Messaging;

namespace DateAndTimeCalculator;

public partial class OpenICS : ContentPage
{

	public OpenICS()
	{
		InitializeComponent();
	}

#if WINDOWS
	protected override void OnSizeAllocated(double width, double height)
	{
		base.OnSizeAllocated(width, height);

		TotalStack.Scale = 1.0f / TotalStack.Scale;

		double WidthFactor = width / TotalStack.Width;
		double HeightFactor = height / TotalStack.Height;

		if (WidthFactor < HeightFactor)
		{
			TotalStack.Scale = WidthFactor;
		}
		else
		{
			TotalStack.Scale = HeightFactor;
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