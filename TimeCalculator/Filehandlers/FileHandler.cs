using CommunityToolkit.Maui.Storage;
using TimeCalculator.MessageThings;

namespace TimeCalculator.FileHandlers;

public partial class FileHandler
{
	//readonly IFileSaver fileSaver;

	//public FileHandler(IFileSaver fileSaver)
	//{
	//	this.fileSaver = fileSaver;
	//}

	public static async Task<SelectFilesResult> SelectFiles(string filetype)
	{
		try
		{
			//#if __MACCATALYST__
			var customFileType = new FilePickerFileType(
							new Dictionary<DevicePlatform, IEnumerable<string>>
							{
								{ DevicePlatform.MacCatalyst,   new[] { filetype } }, // UTType values
								{ DevicePlatform.WinUI,         new[] { filetype } }, // UTType values
								{ DevicePlatform.iOS,           new[] { filetype } }, // UTType values
								{ DevicePlatform.Android,       new[] { filetype } }, // UTType values
								{ DevicePlatform.Tizen,         new[] { filetype } }, // UTType values
                            });
			PickOptions options = new()
			{
				PickerTitle = "Please select file(s)"
				,
				FileTypes = customFileType
			};
			//#else
			//			PickOptions options = new()
			//			{
			//				PickerTitle = "Please select file(s)"
			//			};
			//#endif

			FileResult pickResult = await FilePicker.PickAsync(options);

			if (pickResult != null)
			{
				var args = new SelectFilesResult();

				args.DidPick = true;

				args.pickResult = pickResult;

				return args;

			}
			else
			{
				return null;
			}
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
