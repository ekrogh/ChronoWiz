using ChronoWiz.Shared.Ics;
using System;
using System.Windows.Input;

namespace ChronoWiz.Avalonia.Linux.ViewModels;

public sealed class SaveToIcsViewModel : ViewModelBase
{
	private readonly IcsCoordinator _coordinator;
	public MainWindowViewModel? Host { get; init; }

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
	public ICommand GoBackCommand { get; }

	public SaveToIcsViewModel()
		: this(IcsCoordinator.Default)
	{
		SaveCommand = new RelayCommand(Save);
	}

	public SaveToIcsViewModel(IcsCoordinator coordinator)
	{
		_coordinator = coordinator;
		SaveCommand = new RelayCommand(Save);
		GoBackCommand = new RelayCommand(() => Navigation?.GoBack(), () => Navigation?.CanGoBack ?? false);
	}

	private void Save()
	{
		_ = SaveAsync();
	}

	private async System.Threading.Tasks.Task SaveAsync()
	{
		if (Host?.PickAndSaveIcsAsync is null)
		{
			Status = "File picker not ready.";
			return;
		}

		var summary = string.IsNullOrWhiteSpace(Summary) ? "Summary" : Summary.Trim();
		var description = string.IsNullOrWhiteSpace(Description) ? "Description" : Description.Trim();
		var location = string.IsNullOrWhiteSpace(Location) ? "Location" : Location.Trim();

		_coordinator.RequestSave(new SaveIcsRequest { Summary = summary, Description = description, Location = location });
		Status = "Saving...";
		try
		{
			var start = Host.StartDate?.Date ?? DateTimeOffset.Now.Date;
			var end = Host.EndDate?.Date ?? DateTimeOffset.Now.Date;
			var ics = IcsGenerator.GenerateCalendar(start, end, summary, description, location);
			var saved = await Host.PickAndSaveIcsAsync(ics);
			Status = saved ? "Saved." : "Save canceled.";
			if (saved)
			{
				Navigation?.GoBack();
			}
		}
		catch (Exception ex)
		{
			Status = $"Save failed: {ex.Message}";
		}
	}
}
