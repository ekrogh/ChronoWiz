using OpenMaui.Platform.Linux.Hosting;
using System.Runtime.InteropServices;

if (string.Equals(Environment.GetEnvironmentVariable("CHRONOWIZ_LINUX_DIAG"), "1", StringComparison.OrdinalIgnoreCase))
{
	Console.WriteLine($"OS: {RuntimeInformation.OSDescription}");
	Console.WriteLine($"Arch: {RuntimeInformation.ProcessArchitecture}");
	Console.WriteLine($"WSL_DISTRO_NAME: {Environment.GetEnvironmentVariable("WSL_DISTRO_NAME")}");
	Console.WriteLine($"WSL_INTEROP: {Environment.GetEnvironmentVariable("WSL_INTEROP")}");
	Console.WriteLine($"DISPLAY: {Environment.GetEnvironmentVariable("DISPLAY")}");
	Console.WriteLine($"WAYLAND_DISPLAY: {Environment.GetEnvironmentVariable("WAYLAND_DISPLAY")}");
	Console.WriteLine($"LD_LIBRARY_PATH: {Environment.GetEnvironmentVariable("LD_LIBRARY_PATH")}");

	try
	{
		Console.WriteLine($"libX11 exists: {File.Exists("/lib/x86_64-linux-gnu/libX11.so.6")}");
	}
	catch (Exception ex)
	{
		Console.WriteLine($"libX11 exists probe failed: {ex.GetType().Name}: {ex.Message}");
	}
}

var app = global::ChronoWiz.Ui.MauiProgram.CreateMauiApp();
Microsoft.Maui.Platform.Linux.LinuxApplication.Run(app, args, o => o.Title = "ChronoWiz");
