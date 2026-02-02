namespace ChronoWiz.Avalonia.Linux.ViewModels;

public sealed class FileIcsViewModel : ViewModelBase
{
	public string Title => "ICS Filing";

	private string _status = "Ready.";
	public string Status
	{
		get => _status;
		set => SetField(ref _status, value);
	}
}
