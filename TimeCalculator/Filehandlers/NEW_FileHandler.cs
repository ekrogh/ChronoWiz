using CommunityToolkit.Maui.Storage;
using PdfCalculator.MessageThings;

namespace PdfCalculator.FileHandlers;

public partial class NEW_FileHandler
{
	readonly IFileSaver fileSaver;

	public NEW_FileHandler(IFileSaver fileSaver)
	{
		this.fileSaver = fileSaver;
	}

	public async Task<SelectFilesResult> SelectFilesToReadFrom(string[] filetypes)
	{
		try
		{
#if __MACCATALYST__
			var customFileType = new FilePickerFileType(
							new Dictionary<DevicePlatform, IEnumerable<string>>
							{
					{ DevicePlatform.MacCatalyst, new[] { "Pdf" } }, // UTType values
                            });
			PickOptions options = new()
			{
				PickerTitle = "Please select file(s)"
				,
				FileTypes = customFileType
			};
#else
			PickOptions options = new()
			{
				PickerTitle = "Please select file(s)"
				,
				FileTypes = FilePickerFileType.Pdf
			};
#endif
			var pickResult = await FilePicker.PickMultipleAsync(options);

			if (pickResult != null)
			{
				var args = new SelectFilesResult();

				args.DidPick = true;

				args.TheSelectedFilesInfo = new List<SelectedFileInfo>();

				SelectedFileInfo urlHere = new SelectedFileInfo();

				foreach (var file in pickResult)
				{
					// Open the document for read
					urlHere.TheStream = await file.OpenReadAsync(); ;

					args.TheSelectedFilesInfo.Add(urlHere);
				}

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

	public async Task<FileSaverResult> SaveToTextFile(MemoryStream TheStream, string filename)
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
