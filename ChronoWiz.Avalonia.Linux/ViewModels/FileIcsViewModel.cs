namespace ChronoWiz.Avalonia.Linux.ViewModels;

public sealed class FileIcsViewModel : ViewModelBase
{
	public string Title => "ICS Filing";
	public System.Windows.Input.ICommand GoBackCommand { get; }
	public System.Windows.Input.ICommand GoOpenIcsCommand { get; }
	public System.Windows.Input.ICommand GoSaveIcsCommand { get; }

	private string _status = "Ready.";
	public string Status
	{
		get => _status;
		set => SetField(ref _status, value);
	}

	public FileIcsViewModel()
	{
		GoBackCommand = new RelayCommand(() => Navigation?.GoBack(), () => Navigation?.CanGoBack ?? false);
		GoOpenIcsCommand = new RelayCommand(() => Navigation?.NavigateTo(MainWindowViewModel.RouteOpenIcs));
		GoSaveIcsCommand = new RelayCommand(() => Navigation?.NavigateTo(MainWindowViewModel.RouteSaveIcs));
	}
}
