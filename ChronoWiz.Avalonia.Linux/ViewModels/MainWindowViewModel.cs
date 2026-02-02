using ChronoWiz.Avalonia.Linux.Navigation;
using System;
using System.Windows.Input;

namespace ChronoWiz.Avalonia.Linux.ViewModels;

public sealed class MainWindowViewModel : ViewModelBase
{
	public const string RouteMain = "Main";
	public const string RouteHelp = "Help";
	public const string RouteOpenIcs = "OpenIcs";
	public const string RouteSaveIcs = "SaveIcs";
	public const string RouteFileIcs = "FileIcs";

	private DateTimeOffset? _startDate = DateTimeOffset.Now.Date;
	private DateTimeOffset? _endDate = DateTimeOffset.Now.Date;
	private string _resultText = "";
	private int _startHourValue = DateTimeOffset.Now.Hour;
	private int _startMinuteValue = DateTimeOffset.Now.Minute;
	private int _endHourValue = DateTimeOffset.Now.Hour;
	private int _endMinuteValue = DateTimeOffset.Now.Minute;
	private object? _currentPage;
	private readonly System.Collections.Generic.Stack<string> _navigationStack = new();
	private string _currentRoute = RouteMain;

	public object? CurrentPage
	{
		get => _currentPage;
		set => SetField(ref _currentPage, value);
	}

	public INavigationService Navigation { get; }

	public ICommand GoMainCommand { get; }
	public ICommand GoHelpCommand { get; }
	public ICommand GoOpenIcsCommand { get; }
	public ICommand GoSaveIcsCommand { get; }
	public ICommand GoFileIcsCommand { get; }
	public ICommand GoBackCommand { get; }

	public ICommand OpenIcsFileCommand { get; }
	public ICommand SaveIcsFileCommand { get; }

	public Func<bool, System.Threading.Tasks.Task<ChronoWiz.Shared.Ics.IcsParseResult?>>? PickAndReadIcsAsync { get; set; }
	public Func<string, System.Threading.Tasks.Task<bool>>? PickAndSaveIcsAsync { get; set; }

	public DateTimeOffset? StartDate
	{
		get => _startDate;
		set
		{
			if (SetField(ref _startDate, value))
			{
				RaisePropertyChanged(nameof(StartDayName));
			}
		}
	}

	public DateTimeOffset? EndDate
	{
		get => _endDate;
		set
		{
			if (SetField(ref _endDate, value))
			{
				RaisePropertyChanged(nameof(EndDayName));
			}
		}
	}

	public int StartHourValue { get => _startHourValue; set => SetField(ref _startHourValue, Clamp(value, 0, 23)); }
	public int StartMinuteValue { get => _startMinuteValue; set => SetField(ref _startMinuteValue, Clamp(value, 0, 59)); }
	public int EndHourValue { get => _endHourValue; set => SetField(ref _endHourValue, Clamp(value, 0, 23)); }
	public int EndMinuteValue { get => _endMinuteValue; set => SetField(ref _endMinuteValue, Clamp(value, 0, 59)); }

	private TimeSpan? _startTime;
	public TimeSpan? StartTime
	{
		get => _startTime ?? new TimeSpan(StartHourValue, StartMinuteValue, 0);
		set
		{
			if (SetField(ref _startTime, value) && value is { } ts)
			{
				StartHourValue = ts.Hours;
				StartMinuteValue = ts.Minutes;
				RaisePropertyChanged(nameof(StartHourText));
				RaisePropertyChanged(nameof(StartMinuteText));
			}
		}
	}

	private TimeSpan? _endTime;
	public TimeSpan? EndTime
	{
		get => _endTime ?? new TimeSpan(EndHourValue, EndMinuteValue, 0);
		set
		{
			if (SetField(ref _endTime, value) && value is { } ts)
			{
				EndHourValue = ts.Hours;
				EndMinuteValue = ts.Minutes;
				RaisePropertyChanged(nameof(EndHourText));
				RaisePropertyChanged(nameof(EndMinuteText));
			}
		}
	}

	public string StartHourText
	{
		get => _startHourValue.ToString("00");
		set
		{
			StartHourValue = ParseAndClamp(value, 0, 23, _startHourValue);
			StartTime = new TimeSpan(StartHourValue, StartMinuteValue, 0);
		}
	}

	public string StartMinuteText
	{
		get => _startMinuteValue.ToString("00");
		set
		{
			StartMinuteValue = ParseAndClamp(value, 0, 59, _startMinuteValue);
			StartTime = new TimeSpan(StartHourValue, StartMinuteValue, 0);
		}
	}

	public string EndHourText
	{
		get => _endHourValue.ToString("00");
		set
		{
			EndHourValue = ParseAndClamp(value, 0, 23, _endHourValue);
			EndTime = new TimeSpan(EndHourValue, EndMinuteValue, 0);
		}
	}

	public string EndMinuteText
	{
		get => _endMinuteValue.ToString("00");
		set
		{
			EndMinuteValue = ParseAndClamp(value, 0, 59, _endMinuteValue);
			EndTime = new TimeSpan(EndHourValue, EndMinuteValue, 0);
		}
	}

	public string ResultText
	{
		get => _resultText;
		set => SetField(ref _resultText, value);
	}

	private string _combinedYears = "";
	public string CombinedYears { get => _combinedYears; set => SetField(ref _combinedYears, value); }
	private string _combinedMonths = "";
	public string CombinedMonths { get => _combinedMonths; set => SetField(ref _combinedMonths, value); }
	private string _combinedWeeks = "";
	public string CombinedWeeks { get => _combinedWeeks; set => SetField(ref _combinedWeeks, value); }
	private string _combinedDays = "";
	public string CombinedDays { get => _combinedDays; set => SetField(ref _combinedDays, value); }
	private string _combinedHours = "";
	public string CombinedHours { get => _combinedHours; set => SetField(ref _combinedHours, value); }
	private string _combinedMinutes = "";
	public string CombinedMinutes { get => _combinedMinutes; set => SetField(ref _combinedMinutes, value); }

	private string _totalYears = "";
	public string TotalYears { get => _totalYears; set => SetField(ref _totalYears, value); }
	private string _totalMonths = "";
	public string TotalMonths { get => _totalMonths; set => SetField(ref _totalMonths, value); }
	private string _totalWeeks = "";
	public string TotalWeeks { get => _totalWeeks; set => SetField(ref _totalWeeks, value); }
	private string _totalDays = "";
	public string TotalDays { get => _totalDays; set => SetField(ref _totalDays, value); }
	private string _totalHours = "";
	public string TotalHours { get => _totalHours; set => SetField(ref _totalHours, value); }
	private string _totalMinutes = "";
	public string TotalMinutes { get => _totalMinutes; set => SetField(ref _totalMinutes, value); }

	public ICommand CalculateCommand { get; }
	public ICommand StartNowCommand { get; }
	public ICommand EndNowCommand { get; }
	public ICommand ClearAllCommand { get; }
	public ICommand CalcCombinedCommand { get; }
	public ICommand CalcStartCommand { get; }
	public ICommand CalcEndCommand { get; }

	public string StartDayName => (StartDate ?? DateTimeOffset.Now).ToString("ddd");
	public string EndDayName => (EndDate ?? DateTimeOffset.Now).ToString("ddd");

	public MainWindowViewModel()
	{
		Navigation = new NavigationService(route => NavigateTo(route), () => _navigationStack.Count > 0, GoBack);

		CalculateCommand = new RelayCommand(Calculate);
		StartNowCommand = new RelayCommand(() =>
		{
			var now = DateTimeOffset.Now;
			StartDate = now.Date;
			StartHourValue = now.Hour;
			StartMinuteValue = now.Minute;
			StartTime = new TimeSpan(StartHourValue, StartMinuteValue, 0);
		});
		EndNowCommand = new RelayCommand(() =>
		{
			var now = DateTimeOffset.Now;
			EndDate = now.Date;
			EndHourValue = now.Hour;
			EndMinuteValue = now.Minute;
			EndTime = new TimeSpan(EndHourValue, EndMinuteValue, 0);
		});
		ClearAllCommand = new RelayCommand(ClearAll);
		CalcCombinedCommand = new RelayCommand(Calculate);
		CalcStartCommand = new RelayCommand(CalculateStartFromEndAndSpan);
		CalcEndCommand = new RelayCommand(CalculateEndFromStartAndSpan);
		GoMainCommand = new RelayCommand(() => NavigateTo(RouteMain));
		GoHelpCommand = new RelayCommand(() => NavigateTo(RouteHelp));
		GoOpenIcsCommand = new RelayCommand(() => NavigateTo(RouteOpenIcs));
		GoSaveIcsCommand = new RelayCommand(() => NavigateTo(RouteSaveIcs));
		GoFileIcsCommand = new RelayCommand(() => NavigateTo(RouteFileIcs));
		GoBackCommand = new RelayCommand(GoBack, () => Navigation.CanGoBack);
		OpenIcsFileCommand = new RelayCommand(() => _ = OpenIcsFileAsync());
		SaveIcsFileCommand = new RelayCommand(() => _ = SaveIcsFileAsync());

		ResultText = "Ready.";
		NavigateTo(RouteMain);
	}

	private void GoBack()
	{
		if (_navigationStack.Count == 0)
		{
			NavigateTo(RouteMain, pushCurrent: false);
			return;
		}

		var route = _navigationStack.Pop();
		NavigateTo(route, pushCurrent: false);
		(GoBackCommand as RelayCommand)?.RaiseCanExecuteChanged();
	}



	private void ClearAll()
	{
		CombinedYears = CombinedMonths = CombinedWeeks = CombinedDays = CombinedHours = CombinedMinutes = "";
		TotalYears = TotalMonths = TotalWeeks = TotalDays = TotalHours = TotalMinutes = "";
		ResultText = "Cleared.";
	}

	private async System.Threading.Tasks.Task OpenIcsFileAsync()
	{
		var correct = ChronoWiz.Shared.Ics.IcsCoordinator.Default.LastOpenRequest?.CorrectForTimeZone ?? false;
		if (PickAndReadIcsAsync is null)
		{
			ResultText = "File picker not ready.";
			return;
		}

		try
		{
			var parsed = await PickAndReadIcsAsync(correct);
			if (parsed is null)
			{
				ResultText = "Open canceled.";
				return;
			}

			StartDate = new DateTimeOffset(parsed.Start);
			EndDate = new DateTimeOffset(parsed.End);
			Calculate();
		}
		catch (Exception ex)
		{
			ResultText = $"Bad .ics file: {ex.Message}";
		}
	}

	private async System.Threading.Tasks.Task SaveIcsFileAsync()
	{
		if (PickAndSaveIcsAsync is null)
		{
			ResultText = "File picker not ready.";
			return;
		}

		var start = StartDate?.Date ?? DateTimeOffset.Now.Date;
		var end = EndDate?.Date ?? DateTimeOffset.Now.Date;
		var req = ChronoWiz.Shared.Ics.IcsCoordinator.Default.LastSaveRequest;
		var summary = req?.Summary ?? "Summary";
		var description = req?.Description ?? "Description";
		var location = req?.Location ?? "Location";
		var ics = ChronoWiz.Shared.Ics.IcsGenerator.GenerateCalendar(start, end, summary, description, location);

		var saved = await PickAndSaveIcsAsync(ics);
		ResultText = saved ? "Saved." : "Save canceled.";
	}

	private void NavigateTo(string route, bool pushCurrent = true)
	{
		if (pushCurrent && route != _currentRoute)
		{
			_navigationStack.Push(_currentRoute);
		}

		_currentRoute = route;
		(GoBackCommand as RelayCommand)?.RaiseCanExecuteChanged();

		CurrentPage = route switch
		{
			RouteMain => this,
			RouteHelp => CreatePage(new AboutHelpViewModel()),
			RouteOpenIcs => CreatePage(new OpenIcsViewModel { Host = this }),
			RouteSaveIcs => CreatePage(new SaveToIcsViewModel { Host = this }),
			RouteFileIcs => CreatePage(new FileIcsViewModel()),
			_ => this
		};
	}

	private T CreatePage<T>(T viewModel) where T : ViewModelBase
	{
		viewModel.Navigation = Navigation;
		return viewModel;
	}

	private void Calculate()
	{
		var startDate = StartDate?.Date ?? DateTimeOffset.Now.Date;
		var endDate = EndDate?.Date ?? DateTimeOffset.Now.Date;
		var start = startDate.Add(new TimeSpan(StartHourValue, StartMinuteValue, 0));
		var end = endDate.Add(new TimeSpan(EndHourValue, EndMinuteValue, 0));
		if (end < start)
		{
			ResultText = "End must be after start.";
			return;
		}

		var calc = CalculateYMWDHM(start, end);
		CombinedYears = calc.CombinedYears.ToString();
		CombinedMonths = calc.CombinedMonths.ToString();
		CombinedWeeks = calc.CombinedWeeks.ToString();
		CombinedDays = calc.CombinedDays.ToString();
		CombinedHours = calc.CombinedHours.ToString();
		CombinedMinutes = calc.CombinedMinutes.ToString();

		TotalYears = calc.TotalYears.ToString();
		TotalMonths = calc.TotalMonths.ToString();
		TotalWeeks = calc.TotalWeeks.ToString();
		TotalDays = calc.TotalDays.ToString();
		TotalHours = calc.TotalHours.ToString();
		TotalMinutes = calc.TotalMinutes.ToString();

		ResultText = "Ready.";
	}

	private void CalculateEndFromStartAndSpan()
	{
		var startDate = StartDate?.Date ?? DateTimeOffset.Now.Date;
		var start = startDate.Add(new TimeSpan(StartHourValue, StartMinuteValue, 0));
		if (!TryGetSpanFromInputs(out var span, out var error))
		{
			ResultText = error;
			return;
		}

		DateTime end;
		try
		{
			end = ApplySpan(start, span);
		}
		catch (ArgumentOutOfRangeException ex)
		{
			ResultText = ex.Message;
			return;
		}

		EndDate = new DateTimeOffset(end.Date);
		EndTime = end.TimeOfDay;
		EndHourValue = end.Hour;
		EndMinuteValue = end.Minute;
		RaisePropertyChanged(nameof(EndHourText));
		RaisePropertyChanged(nameof(EndMinuteText));

		Calculate();
	}

	private void CalculateStartFromEndAndSpan()
	{
		var endDate = EndDate?.Date ?? DateTimeOffset.Now.Date;
		var end = endDate.Add(new TimeSpan(EndHourValue, EndMinuteValue, 0));
		if (!TryGetSpanFromInputs(out var span, out var error))
		{
			ResultText = error;
			return;
		}

		DateTime start;
		try
		{
			start = ApplySpan(end, span, negate: true);
		}
		catch (ArgumentOutOfRangeException ex)
		{
			ResultText = ex.Message;
			return;
		}

		StartDate = new DateTimeOffset(start.Date);
		StartTime = start.TimeOfDay;
		StartHourValue = start.Hour;
		StartMinuteValue = start.Minute;
		RaisePropertyChanged(nameof(StartHourText));
		RaisePropertyChanged(nameof(StartMinuteText));

		Calculate();
	}

	private readonly record struct InputSpan(
		bool IsTotal,
		int Years,
		int Months,
		int Weeks,
		int Days,
		int Hours,
		int Minutes);

	private bool TryGetSpanFromInputs(out InputSpan span, out string error)
	{
		span = default;
		error = "";

		var combined = ParseSpanFromInputs(isTotal: false);
		var total = ParseSpanFromInputs(isTotal: true);

		var combinedAny = combined is not null && HasAnySpanValue(combined.Value);
		var totalAny = total is not null && HasAnySpanValue(total.Value);

		if (combinedAny && totalAny)
		{
			error = "Not both \"Total\" and \"Combined\" time spans can be used.";
			return false;
		}

		if (!combinedAny && !totalAny)
		{
			span = new InputSpan(false, 0, 0, 0, 0, 0, 0);
			return true;
		}

		span = (totalAny ? total : combined)!.Value;
		return true;
	}

	private static bool HasAnySpanValue(InputSpan span) =>
		span.Years != 0 || span.Months != 0 || span.Weeks != 0 || span.Days != 0 || span.Hours != 0 || span.Minutes != 0;

	private InputSpan? ParseSpanFromInputs(bool isTotal)
	{
		bool TryParse(string s, out int v) => int.TryParse(s, out v);

		var yearsText = isTotal ? TotalYears : CombinedYears;
		var monthsText = isTotal ? TotalMonths : CombinedMonths;
		var weeksText = isTotal ? TotalWeeks : CombinedWeeks;
		var daysText = isTotal ? TotalDays : CombinedDays;
		var hoursText = isTotal ? TotalHours : CombinedHours;
		var minutesText = isTotal ? TotalMinutes : CombinedMinutes;

		if (!TryParse(yearsText, out var years)) years = 0;
		if (!TryParse(monthsText, out var months)) months = 0;
		if (!TryParse(weeksText, out var weeks)) weeks = 0;
		if (!TryParse(daysText, out var days)) days = 0;
		if (!TryParse(hoursText, out var hours)) hours = 0;
		if (!TryParse(minutesText, out var minutes)) minutes = 0;

		if (isTotal)
		{
			var countNonZero = 0;
			if (years != 0) countNonZero++;
			if (months != 0) countNonZero++;
			if (weeks != 0) countNonZero++;
			if (days != 0) countNonZero++;
			if (hours != 0) countNonZero++;
			if (minutes != 0) countNonZero++;
			if (countNonZero > 1)
			{
				ResultText = "Only one \"Total\" value allowed.";
				return null;
			}
		}

		return new InputSpan(isTotal, years, months, weeks, days, hours, minutes);
	}

	private static DateTime ApplySpan(DateTime baseDt, InputSpan span, bool negate = false)
	{
		var sign = negate ? -1 : 1;
		var dt = baseDt;
		if (span.IsTotal)
		{
			if (span.Years != 0) return dt.AddYears(sign * span.Years);
			if (span.Months != 0) return dt.AddMonths(sign * span.Months);
			if (span.Weeks != 0) return dt.AddDays(sign * span.Weeks * 7);
			if (span.Days != 0) return dt.AddDays(sign * span.Days);
			if (span.Hours != 0) return dt.AddHours(sign * span.Hours);
			if (span.Minutes != 0) return dt.AddMinutes(sign * span.Minutes);
			return dt;
		}

		dt = dt.AddYears(sign * span.Years);
		dt = dt.AddMonths(sign * span.Months);
		dt = dt.AddDays(sign * span.Weeks * 7);
		dt = dt.AddDays(sign * span.Days);
		dt = dt.AddHours(sign * span.Hours);
		dt = dt.AddMinutes(sign * span.Minutes);
		return dt;
	}

	private readonly record struct TimeSpanCalcResult(
		int CombinedYears,
		int CombinedMonths,
		int CombinedWeeks,
		int CombinedDays,
		int CombinedHours,
		int CombinedMinutes,
		long TotalYears,
		long TotalMonths,
		long TotalWeeks,
		long TotalDays,
		long TotalHours,
		long TotalMinutes);

	private static TimeSpanCalcResult CalculateYMWDHM(DateTime start, DateTime end)
	{
		var years = end.Year - start.Year;
		var months = end.Month - start.Month;
		if (months < 0)
		{
			months += 12;
			years--;
		}

		var dtCalc1 = start.AddYears(years).AddMonths(months);
		var tsYearsMonths = dtCalc1 - start;
		var tsTotal = end - start;

		var days = tsTotal.Days - tsYearsMonths.Days;
		if (days < 0)
		{
			months--;
			if (months < 0)
			{
				months += 12;
				years--;
			}
			var dtCalc2 = start.AddYears(years).AddMonths(months);
			days = (tsTotal - (dtCalc2 - start)).Days;
		}

		var hours = tsTotal.Hours;
		var minutes = tsTotal.Minutes;

		var weeks = days / 7;
		days %= 7;

		var totDays = (long)tsTotal.TotalDays;
		var totWeeks = totDays / 7;
		var totMonths = (long)months + 12L * years;
		var totYears = (long)years;
		var totHours = (long)tsTotal.TotalHours;
		var totMinutes = (long)tsTotal.TotalMinutes;

		return new TimeSpanCalcResult(
			years,
			months,
			weeks,
			days,
			hours,
			minutes,
			totYears,
			totMonths,
			totWeeks,
			totDays,
			totHours,
			totMinutes);
	}

	private static int Clamp(int value, int min, int max) => value < min ? min : (value > max ? max : value);
	private static int ParseAndClamp(string? text, int min, int max, int fallback)
	{
		if (!int.TryParse(text, out var value))
		{
			return fallback;
		}

		return Clamp(value, min, max);
	}
}

internal sealed class RelayCommand : ICommand
{
	private readonly Action _execute;
	private readonly Func<bool>? _canExecute;

	public RelayCommand(Action execute, Func<bool>? canExecute = null)
	{
		_execute = execute;
		_canExecute = canExecute;
	}

	public event EventHandler? CanExecuteChanged;

	public bool CanExecute(object? parameter) => _canExecute?.Invoke() ?? true;

	public void Execute(object? parameter) => _execute();

	public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}
