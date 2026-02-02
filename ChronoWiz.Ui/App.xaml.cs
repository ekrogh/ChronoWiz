namespace ChronoWiz.Ui;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
		if (OperatingSystem.IsLinux())
		{
			MainPage = new View.LinuxMainPage();
		}
		else
		{
			MainPage = new AppShell();
		}
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		if (OperatingSystem.IsLinux())
		{
			return new Window(MainPage);
		}

		return new Window(new AppShell());
	}
}
