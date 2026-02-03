namespace ChronoWiz.Ui;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(ICSFiling.FileICS), typeof(ICSFiling.FileICS));
		Routing.RegisterRoute(nameof(ICSFiling.OpenICS), typeof(ICSFiling.OpenICS));
		Routing.RegisterRoute(nameof(ICSFiling.SaveToICS), typeof(ICSFiling.SaveToICS));
		Routing.RegisterRoute(nameof(AboutHelp.AboutHelp), typeof(AboutHelp.AboutHelp));
	}
}
