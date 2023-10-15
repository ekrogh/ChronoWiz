namespace TimeCalculator
{
	public partial class AppShell : Shell
	{
		public AppShell()
		{
			InitializeComponent();
		
			Routing.RegisterRoute(nameof(FileICS), typeof(FileICS));
			Routing.RegisterRoute(nameof(AboutHelp), typeof(AboutHelp));
		}
	}
}
