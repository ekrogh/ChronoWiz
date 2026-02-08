using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Skia;
using System;

namespace ChronoWiz.Avalonia.Linux;

class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        Environment.SetEnvironmentVariable("AVALONIA_DISABLE_GPU", "1");
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UseX11()
            .UseSkia() // forces Skia renderer; avoids GLX/OpenGL
            .WithInterFont()
            .LogToTrace();
}
