using ChronoWiz.Shared.Ics;
using System.Windows.Input;

namespace ChronoWiz.Avalonia.Linux.ViewModels;

public sealed class SaveToIcsViewModel : ViewModelBase
{
	private readonly IcsCoordinator _coordinator;

	public string Title => "Save to ICS";

	private string? _summary;
	public string? Summary
	{
		get => _summary;
		set => SetField(ref _summary, value);
	}

	private string? _description;
	public string? Description
	{
		get => _description;
		set => SetField(ref _description, value);
	}

	private string? _location;
	public string? Location
	{
		get => _location;
		set => SetField(ref _location, value);
	}

	private string _status = "Ready.";
	public string Status
	{
		get => _status;
		set => SetField(ref _status, value);
	}

	public ICommand SaveCommand { get; }

	public SaveToIcsViewModel()
		: this(IcsCoordinator.Default)
	{
		SaveCommand = new RelayCommand(Save);
	}

	public SaveToIcsViewModel(IcsCoordinator coordinator)
	{
		_coordinator = coordinator;
		SaveCommand = new RelayCommand(Save);
	}

	private void Save()
	{
		var summary = string.IsNullOrWhiteSpace(Summary) ? "Summary" : Summary.Trim();
		var description = string.IsNullOrWhiteSpace(Description) ? "Description" : Description.Trim();
		var location = string.IsNullOrWhiteSpace(Location) ? "Location" : Location.Trim();

		_coordinator.RequestSave(new SaveIcsRequest
		{
			Summary = summary,
			Description = description,
			Location = location
		});

		Status = "Save requested.";
	}
}
