using ChronoWiz.Shared.Ics;
using System.Windows.Input;

namespace ChronoWiz.Avalonia.Linux.ViewModels;

public sealed class OpenIcsViewModel : ViewModelBase
{
	private readonly IcsCoordinator _coordinator;

	public string Title => "Open ICS";

	private bool _correctTimeZone;
	public bool CorrectTimeZone
	{
		get => _correctTimeZone;
		set => SetField(ref _correctTimeZone, value);
	}

	private string _status = "Ready.";
	public string Status
	{
		get => _status;
		set => SetField(ref _status, value);
	}

	public ICommand OpenCommand { get; }

	public OpenIcsViewModel()
		: this(IcsCoordinator.Default)
	{
	}

	public OpenIcsViewModel(IcsCoordinator coordinator)
	{
		_coordinator = coordinator;
		OpenCommand = new RelayCommand(Open);
	}

	private void Open()
	{
		_coordinator.RequestOpen(new OpenIcsRequest
		{
			CorrectForTimeZone = CorrectTimeZone
		});

		Status = "Open requested.";
	}
}
