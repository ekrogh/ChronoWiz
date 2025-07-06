namespace ChronoWiz;

public partial class FileICS : ContentPage
{
	public FileICS()
	{
		try
		{
			InitializeComponent();
		}
		catch (Exception ex)
		{
			var tst = ex;
		}
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

	private async void OpenICSButton_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync
		(
			nameof(OpenICS)
			, true
		);
	}

	private async void SaveToICSButton_ClickedAsync(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync
		(
			nameof(SaveToICS)
			, true
		);
	}
}