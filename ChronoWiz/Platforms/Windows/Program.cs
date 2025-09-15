using Microsoft.UI.Xaml;

namespace ChronoWiz.WinUI;

public static class Program
{
	[STAThread]
	static void Main(string[] args)
	{
		// Use MAUI bootstrap for WinUI to ensure proper initialization during debugging
		Microsoft.Maui.MauiWinUIApplication.Start(app => new App());
	}
}