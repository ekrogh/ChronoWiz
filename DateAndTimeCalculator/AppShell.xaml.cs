namespace DateAndTimeCalculator
{
	public partial class AppShell : Shell
	{
		public AppShell()
		{
			InitializeComponent();
		
			Routing.RegisterRoute(nameof(FileICS), typeof(FileICS));
			Routing.RegisterRoute(nameof(OpenICS), typeof(OpenICS));
			Routing.RegisterRoute(nameof(SaveToICS), typeof(SaveToICS));
			Routing.RegisterRoute(nameof(AboutHelp), typeof(AboutHelp));
		}
	}
}
