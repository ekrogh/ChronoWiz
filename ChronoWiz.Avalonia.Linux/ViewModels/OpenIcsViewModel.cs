using ChronoWiz.Shared.Ics;
using System;
using System.Windows.Input;

namespace ChronoWiz.Avalonia.Linux.ViewModels;

public sealed class OpenIcsViewModel : ViewModelBase
{
	private readonly IcsCoordinator _coordinator;
	public MainWindowViewModel? Host { get; init; }

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
	public ICommand GoBackCommand { get; }

	public OpenIcsViewModel()
		: this(IcsCoordinator.Default)
	{
	}

	public OpenIcsViewModel(IcsCoordinator coordinator)
	{
		_coordinator = coordinator;
		OpenCommand = new RelayCommand(Open);
		GoBackCommand = new RelayCommand(() => Navigation?.GoBack(), () => Navigation?.CanGoBack ?? false);
	}

	private void Open()
	{
		_ = OpenAsync();
	}

	private async System.Threading.Tasks.Task OpenAsync()
	{
		if (Host?.PickAndReadIcsAsync is null)
		{
			Status = "File picker not ready.";
			return;
		}

		_coordinator.RequestOpen(new OpenIcsRequest { CorrectForTimeZone = CorrectTimeZone });
		Status = "Opening...";
		try
		{
			var parsed = await Host.PickAndReadIcsAsync(CorrectTimeZone);
			if (parsed is null)
			{
				Status = "Open canceled.";
				return;
			}

			Host.StartDate = new DateTimeOffset(parsed.Start);
			Host.EndDate = new DateTimeOffset(parsed.End);
			Host.StartTime = parsed.Start.TimeOfDay;
			Host.EndTime = parsed.End.TimeOfDay;
			Host.CalculateCommand.Execute(null);
			Status = "Opened.";
			Navigation?.GoBack();
		}
		catch (Exception ex)
		{
			Status = $"Bad .ics file: {ex.Message}";
		}
	}
}
