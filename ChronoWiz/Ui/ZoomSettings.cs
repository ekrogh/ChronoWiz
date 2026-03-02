using Microsoft.Maui.Storage;
using Microsoft.Maui.Devices;

namespace ChronoWiz.Ui;

internal static class ZoomSettings
{
	private const string ZoomToWindowKey = "ZoomToWindowEnabled";

	public static bool IsZoomSupportedForCurrentDevice => DeviceInfo.Idiom != DeviceIdiom.Phone;

	public static bool IsZoomToWindowEnabled
	{
      get => Preferences.Default.Get(ZoomToWindowKey, false);
		set => Preferences.Default.Set(ZoomToWindowKey, value);
	}
}
