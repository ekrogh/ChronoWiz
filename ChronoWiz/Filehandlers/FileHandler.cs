using Microsoft.Maui.Storage;
using Microsoft.Maui.Devices;
namespace ChronoWiz.FileHandlers;

public partial class OLD_FileHandler
{
	public static async Task<SelectFilesResult> SelectFiles(string filetype)
	{
		try
		{
			var customFileType = new FilePickerFileType(
				new Dictionary<DevicePlatform, IEnumerable<string>>
				{
					{ DevicePlatform.iOS, new[] { "com.apple.ical.ics" } }, // UTType values
                    { DevicePlatform.Android, new[] { "text/calendar" } }, // MIME type
                    { DevicePlatform.WinUI, new[] { ".ics" } }, // file extension
                    { DevicePlatform.Tizen, new[] { "*/*" } },
					{ DevicePlatform.MacCatalyst, new[] { "com.apple.ical.ics" } }, // UTType values
					{ DevicePlatform.macOS, new[] { "com.apple.ical.ics" } }, // UTType values
                });

			PickOptions options = new()
			{
				PickerTitle = "Please select file(s)"
				,
				FileTypes = customFileType
			};

			FileResult pickResult = await FilePicker.PickAsync(options);

			var args = new SelectFilesResult();

			if (pickResult != null)
			{
				args.DidPick = true;
			}
			else
			{
				args.DidPick = false;
			}

			args.pickResult = pickResult;

			return args;
		}
		catch (Exception ex)
		{
			var msg = $"File(s) not selected, {ex.ToString()}";
			await Shell.Current.DisplayAlert("Error", msg, "OK");
			return null;
		}
	}

	public static async Task<FileSaverResult> SaveToTextFile(MemoryStream TheStream, string filename)
	{
		try
		{
			return await FileSaver.Default.SaveAsync(filename, TheStream, default);
		}
		catch (Exception e)
		{
			var msg = $"File is not saved, {e.ToString()}";
			await Shell.Current.DisplayAlert("Error", msg, "OK");
			System.Diagnostics.Debug.WriteLine("SaveToTextFile failed: {0}", e.ToString());
			return null;
		}
		finally
		{
		}
	}

}
