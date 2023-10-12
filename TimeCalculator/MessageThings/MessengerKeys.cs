namespace TimeCalculator.MessageThings;

public class MessengerKeys
{
	// Orientation msgs.
	public static string LandscapeOrientationRequest { get; } = "LandscapeOrientationRequest";
	public static string PortraitOrientationRequest { get; } = "PortraitOrientationRequest";
	public static string AllButUpsideDowntOrientationRequest { get; } = "AllButUpsideDowntOrientationRequest";

	// File handl. msgs.
	public static string FileToReadFromSelected { get; } = "FileToReadFromSelected";
	public static string FileToSaveToSelected { get; } = "FileToSaveToSelected";
	public static string FileToSaveRawTextToSelected { get; } = "FileToSaveRawTextToSelected";

	// Open/Save To .ics Description entered
	public static string SaveToIcsMessageKey { get; } = "SaveToIcsMessageKey";
	public static string OpenIcsMessageKey { get; } = "OpenIcsMessageKey";


}
