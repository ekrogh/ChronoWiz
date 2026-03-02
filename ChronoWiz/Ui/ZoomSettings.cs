using Microsoft.Maui.Storage;

namespace ChronoWiz.Ui;

internal static class ZoomSettings
{
	private const string ZoomToWindowKey = "ZoomToWindowEnabled";

	public static bool IsZoomToWindowEnabled
	{
      get => Preferences.Default.Get(ZoomToWindowKey, false);
		set => Preferences.Default.Set(ZoomToWindowKey, value);
	}
}
