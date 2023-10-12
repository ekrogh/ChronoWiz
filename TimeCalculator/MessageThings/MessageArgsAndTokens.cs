namespace TimeCalculator;

// .ics file
public class SaveToIcsMessageArgs : EventArgs
{
	public string TheDescription;
	public string EventName_Summary;
	public string Location;
}
public class OpenIcsMessageArgs : EventArgs
{
	public bool CorrectForTimeZone = false;
}

// Select file
public struct SelectedFileInfo
{
	public System.IO.Stream TheStream { get; set; }
}
public class SelectFileResultMessageArgs : EventArgs
{
	public bool DidPick { get; set; }
	public SelectedFileInfo TheSelectedFileInfo { get; set; }
}
public sealed class SelectFilesResult
{
	public bool DidPick { get; set; }
	public List<SelectedFileInfo> TheSelectedFilesInfo { get; set; }
}

// Message keys
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
