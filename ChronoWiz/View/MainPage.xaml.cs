using ChronoWiz.FileHandlers;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace ChronoWiz.View;

// Learn more about making custom code visible in the Xamarin.Forms previewer
// by visiting https://aka.ms/xamarinforms-previewer
[DesignTimeVisible(true)]
public partial class MainPage : ContentPage
{
	private Ui.ZoomToWindowController? _zoomToWindow;
	private bool _displayInfoSubscribed;
    private bool _initialDisplayInfoApplied;

    public MainPage()
    {
        InitializeComponent();
        BindingContext = this;

        WeakReferenceMessenger.Default.Register<SaveToIcsMessageArgs, string>
            (this, MessengerKeys.SaveToIcsMessageKey, On_SaveToIcsMessageReceived);

        WeakReferenceMessenger.Default.Register<OpenIcsMessageArgs, string>
            (this, MessengerKeys.OpenIcsMessageKey, On_OpenIcsMessageReceived);


        Resources["DynamicBaseButtonStyle"] = Resources["baseButtonStyle"];

        DictionaryOfCombinedEntries = new Dictionary<Entry, int>()
        {
            { CombndYears,      0 },
            { CombndMonths,     0 },
            { CombndWeeks,      0 },
            { CombndDays,       0 },
            { CombndHours,      0 },
            { CombndMinutes,    0 }
        };
        DictionaryOfTotalEntries = new Dictionary<Entry, int>()
        {
            { TotYears,     0 },
            { TotMonths,    0 },
            { TotWeeks,     0 },
            { TotDays,      0 },
            { TotHours,     0 },
            { TotMinutes,   0 }
        };

        StartDateIn = DateTime.Now.Date;
        StartTimeIn = DateTime.Now.TimeOfDay;

        EndDateIn = DateTime.Now.Date;
        EndTimeIn = DateTime.Now.TimeOfDay;

        StartDatePicker.Format = CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern;
        StartTimePicker.Format = CultureInfo.CurrentUICulture.DateTimeFormat.ShortTimePattern;

        EndDatePicker.Format = CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern;
        EndTimePicker.Format = CultureInfo.CurrentUICulture.DateTimeFormat.ShortTimePattern;

        StartTimePicker.Time = DateTime.Now.TimeOfDay;
        EndTimePicker.Time = DateTime.Now.TimeOfDay;

        StartDatePicker.Date = DateTime.Now.Date;
        EndDatePicker.Date = DateTime.Now.Date;

		// Hook/unhook in OnAppearing/OnDisappearing so navigation away and back keeps layout updates working.
    }

	protected override void OnAppearing()
	{
		base.OnAppearing();

		if (!_displayInfoSubscribed)
		{
			DeviceDisplay.Current.MainDisplayInfoChanged += Current_MainDisplayInfoChanged;
			_displayInfoSubscribed = true;
		}

      SyncZoomController();
        Dispatcher.Dispatch(ApplyCurrentDisplayInfo);
	}

	protected override void OnDisappearing()
	{
		if (_displayInfoSubscribed)
		{
			DeviceDisplay.Current.MainDisplayInfoChanged -= Current_MainDisplayInfoChanged;
			_displayInfoSubscribed = false;
		}
		_zoomToWindow?.Dispose();
		_zoomToWindow = null;
		base.OnDisappearing();
	}

    private void Current_MainDisplayInfoChanged(object? sender, DisplayInfoChangedEventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
			ApplyOrientationAndZoom(e.DisplayInfo.Orientation);
        });
    }

    private void ApplyCurrentDisplayInfo()
    {
#if __MACCATALYST__
        ApplyOrientationAndZoom(DisplayOrientation.Landscape, clearAll: !_initialDisplayInfoApplied);
        _initialDisplayInfoApplied = true;
#else
        try
        {
            var info = DeviceDisplay.Current.MainDisplayInfo;
            ApplyOrientationAndZoom(info.Orientation, clearAll: !_initialDisplayInfoApplied);
            _initialDisplayInfoApplied = true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Unable to read display info during startup: {ex}");
        }
#endif
    }

	private void ApplyOrientationAndZoom(DisplayOrientation orientation, bool clearAll = true)
	{
		if (_zoomToWindow is null)
		{
			SetOrientationRight(orientation, clearAll);
			return;
		}

		_zoomToWindow.BeginLayoutChange();
		try
		{
			SetOrientationRight(orientation, clearAll);
		}
		finally
		{
			_zoomToWindow.EndLayoutChange();
		}
	}

    private void SyncZoomController()
    {
        if (Ui.ZoomSettings.IsZoomSupportedForCurrentDevice && Ui.ZoomSettings.IsZoomToWindowEnabled)
        {
            _zoomToWindow ??= new Ui.ZoomToWindowController(this, TotalStackName);
        }
        else
        {
            _zoomToWindow?.Dispose();
            _zoomToWindow = null;
            TotalStackName.Scale = 1.0;
        }
    }

	private void SetOrientationRight(DisplayOrientation DipsOrient, bool clearAll = true)
    {
        bool portrait = (DipsOrient == DisplayOrientation.Portrait);

        TotalStackName.TranslationX = 0.0f;
        TotalStackName.TranslationY = 0.0f;

        if (firstTimeWdthOrHeightChanged)
        {
            StartDateTimeIntroLabelName.FontSize = StartDateTimeIntroLabelNameFontSizeOrig;
            StartDayName.FontSize = StartEndDayNameFontSizeOrig;
            firstTimeWdthOrHeightChanged = false;
        }

        if (portrait)
        {
            EntriesCenterOuterStack.Orientation = StackOrientation.Horizontal;
            EntriesCenterCombndStack.Orientation = StackOrientation.Vertical;
            EntriesCenterTotStack.Orientation = StackOrientation.Vertical;
        }
        else
        {
            EntriesCenterOuterStack.Orientation = StackOrientation.Vertical;
            EntriesCenterCombndStack.Orientation = StackOrientation.Horizontal;
            EntriesCenterTotStack.Orientation = StackOrientation.Horizontal;
        }

        if (DeviceInfo.Platform == DevicePlatform.MacCatalyst)
        {
            StartLabelNDateTimeStack.Orientation = StackOrientation.Horizontal;
            EndLabelNDateTimeStack.Orientation = StackOrientation.Horizontal;
        }
        else if (DeviceInfo.Platform == DevicePlatform.Android)
        {
            if (portrait)
            {
                StartLabelNDateTimeStack.Orientation = StackOrientation.Vertical;
                EndLabelNDateTimeStack.Orientation = StackOrientation.Vertical;
            }
            else
            {
                StartLabelNDateTimeStack.Orientation = StackOrientation.Horizontal;
                EndLabelNDateTimeStack.Orientation = StackOrientation.Horizontal;
            }

            StartDayName.WidthRequest = EndDayName.WidthRequest = 50;
        }
        else if (DeviceInfo.Platform == DevicePlatform.iOS)
        {
            if (portrait)
            {
                StartLabelNDateTimeStack.Orientation = StackOrientation.Vertical;
                EndLabelNDateTimeStack.Orientation = StackOrientation.Vertical;
            }
            else
            {
                StartLabelNDateTimeStack.Orientation = StackOrientation.Horizontal;
                EndLabelNDateTimeStack.Orientation = StackOrientation.Horizontal;
            }
        }
        else if (DeviceInfo.Platform == DevicePlatform.WinUI)
        {
            StartLabelNDateTimeStack.Orientation = StackOrientation.Horizontal;
            EndLabelNDateTimeStack.Orientation = StackOrientation.Horizontal;
            StartDayName.WidthRequest = EndDayName.WidthRequest = 45;
        }

		if (clearAll)
			DoClearAll();
		else
			ApplyEntryWidthRequests();
    }

    private bool firstTimeWdthOrHeightChanged = true;

    DatePicker MacStartDatePicker = new DatePicker();
    DatePicker MacEndDatePicker = new DatePicker();
    Picker GtkStartHourPicker = new Picker();
    Picker GtkStartMinutsPicker = new Picker();
    Picker GtkEndHourPicker = new Picker();
    Picker GtkEndMinutsPicker = new Picker();

    private double StartDateTimeIntroLabelNameFontSizeOrig = 0.0;
    private double StartEndDayNameFontSizeOrig = 0.0;

    public DateTime StartDateTimeIn { get; set; }
    public DateTime StartDateIn { get; set; }
    public TimeSpan StartTimeIn { get; set; }
    public bool DoCalcStartTime { get; set; } = false;
    public DateTime StartDateTimeOut { get; set; }
    public DateTime EndDateTimeIn { get; set; }
    public DateTime EndDateIn { get; set; }
    public TimeSpan EndTimeIn { get; set; }
    public bool DoCalcEndTime { get; set; } = false;
    public DateTime EndDateTimeOut { get; set; }
    public bool DoCalcYMWDHM { get; set; } = true;

    private TimeSpan PrivEnteredYMWDHMTimeSpan { get; set; } = new TimeSpan(0);
    public TimeSpan EnteredYMWDHMTimeSpan
    {
        get => PrivEnteredYMWDHMTimeSpan;
        set => PrivEnteredYMWDHMTimeSpan = value;
    }

    private Dictionary<Entry, int> DictionaryOfCombinedEntries;
    public Dictionary<Entry, int> DictionaryOfTotalEntries;

    private int CombndYearsOut = 0;
    private int CombndMonthsOut = 0;
    private int CombndWeeksOut = 0;
    private int CombndDaysOut = 0;
    private int CombndHoursOut = 0;
    private int CombndMinutesOut = 0;
    private long TotYearsOut = 0;
    private long TotMonthsOut = 0;
    private long TotWeeksOut = 0;
    private long TotDaysOut = 0;
    private long TotHoursOut = 0;
    private long TotMinutesOut = 0;

    private void SetStartDateTime()
    {
        try
        {
            StartTimePicker.Time = new TimeSpan(StartTimeIn.Hours, StartTimeIn.Minutes, 0);
            StartDatePicker.Date = StartDateIn;
            StartDayName.Text = StartDateIn.DayOfWeek.ToString().Remove(3);
        }
        catch (Exception) { }
    }

    private void SetEndDateTime()
    {
        try
        {
            EndTimePicker.Time = new TimeSpan(EndTimeIn.Hours, EndTimeIn.Minutes, 0);
            EndDatePicker.Date = EndDateIn;
            EndDayName.Text = EndDateIn.DayOfWeek.ToString().Remove(3);
        }
        catch (Exception) { }
    }

    private void ClearTotIOVars(Entry? ImInFocus)
    {
        foreach (Entry entry in DictionaryOfTotalEntries.Keys)
        {
            if (entry != ImInFocus)
                DictionaryOfTotalEntries[entry] = 0;
        }
        TotYearsOut = 0; TotMonthsOut = 0; TotWeeksOut = 0; TotDaysOut = 0; TotHoursOut = 0; TotMinutesOut = 0;
    }

    private void ClearCombinedIOVars(Entry? ImInFocus)
    {
        foreach (Entry entry in DictionaryOfCombinedEntries.Keys)
        {
            if (entry != ImInFocus)
                DictionaryOfCombinedEntries[entry] = 0;
        }
        CombndYearsOut = 0; CombndMonthsOut = 0; CombndWeeksOut = 0; CombndDaysOut = 0; CombndHoursOut = 0; CombndMinutesOut = 0;
    }

    private void ClearCombinedYMWDHM(Entry? ImInFocus)
    {
        foreach (Entry CurEntry in DictionaryOfCombinedEntries.Keys)
        {
            if (CurEntry != ImInFocus)
                CurEntry.Text = "";
        }
        ClearCombinedIOVars(ImInFocus);
    }

    private void ClearTotYMWDHM(Entry? ImInFocus)
    {
        foreach (Entry CurEntry in DictionaryOfTotalEntries.Keys)
        {
            if (CurEntry != ImInFocus)
                CurEntry.Text = "";
        }
        ClearTotIOVars(ImInFocus);
    }

    private void ClearYMWDHM(Entry? ImInFocus)
    {
        ClearCombinedYMWDHM(ImInFocus);
        ClearTotYMWDHM(ImInFocus);
    }

    private void ClearAllIOVars()
    {
        ClearTotIOVars(null);
        ClearCombinedIOVars(null);
    }

    private void DoClearAll()
    {
        SetStartDateTime();
        SetEndDateTime();
        ClearYMWDHM(null);

		ApplyEntryWidthRequests();

        ClearAllIOVars();
    }

	private void ApplyEntryWidthRequests()
	{
		if (DeviceInfo.Platform == DevicePlatform.iOS)
		{
			CombndYears.WidthRequest = CombndMonths.WidthRequest = CombndWeeks.WidthRequest = CombndDays.WidthRequest = CombndHours.WidthRequest = CombndMinutes.WidthRequest = 105;
			TotYears.WidthRequest = TotMonths.WidthRequest = TotWeeks.WidthRequest = TotDays.WidthRequest = TotHours.WidthRequest = TotMinutes.WidthRequest = 105;
		}
		else if (DeviceInfo.Platform == DevicePlatform.Android)
		{
			CombndYears.WidthRequest = CombndMonths.WidthRequest = CombndWeeks.WidthRequest = CombndDays.WidthRequest = CombndHours.WidthRequest = CombndMinutes.WidthRequest = 88;
			TotYears.WidthRequest = TotMonths.WidthRequest = TotWeeks.WidthRequest = TotDays.WidthRequest = TotHours.WidthRequest = TotMinutes.WidthRequest = 88;
		}
		else if (DeviceInfo.Platform == DevicePlatform.WinUI)
		{
			CombndYears.WidthRequest = CombndMonths.WidthRequest = CombndWeeks.WidthRequest = CombndDays.WidthRequest = CombndHours.WidthRequest = CombndMinutes.WidthRequest = 121;
			TotYears.WidthRequest = TotMonths.WidthRequest = TotWeeks.WidthRequest = TotDays.WidthRequest = TotHours.WidthRequest = TotMinutes.WidthRequest = 121;
		}
		else
		{
			CombndYears.WidthRequest = CombndMonths.WidthRequest = CombndWeeks.WidthRequest = CombndDays.WidthRequest = CombndHours.WidthRequest = CombndMinutes.WidthRequest = 121;
			TotYears.WidthRequest = TotMonths.WidthRequest = TotWeeks.WidthRequest = TotDays.WidthRequest = TotHours.WidthRequest = TotMinutes.WidthRequest = 121;
		}
	}

    // Start date-time...

    [RelayCommand]
    public void CalcStartTimeBtnClicked()
    {
        DoCalcStartTime = true;
        DoCalcEndTime = false;
        DoCalcYMWDHM = false;
        LabelEqual.Text = "-";
        LabelPlus.Text = "=";
        DoCalculate();
    }

    private void CheckSetEndDateTime()
    {
        if (EndDateIn < StartDateIn)
        {
            EndDateIn = StartDateIn;
            EndTimeIn = StartTimeIn;
            SetEndDateTime();
        }
        else if ((EndDateIn == StartDateIn) && (EndTimeIn < StartTimeIn))
        {
            EndTimeIn = StartTimeIn;
            SetEndDateTime();
        }
    }

    private void StartDatePicker_DateSelected(object sender, DateChangedEventArgs e)
    {
        StartDateIn = e.NewDate.GetValueOrDefault();
        MacStartDatePicker.Date = StartDateIn;
        StartDayName.Text = StartDateIn.DayOfWeek.ToString().Remove(3);
        CheckSetEndDateTime();
    }

    private void OnMacStartDatePickerDateSelected(object sEnder, DateChangedEventArgs e)
    {
        StartDateIn = e.NewDate.GetValueOrDefault();
        StartDatePicker.Date = StartDateIn;
        StartDayName.Text = StartDateIn.DayOfWeek.ToString().Remove(3);
        CheckSetEndDateTime();
    }

    private void OnMacStartTimePickerPropertyChanged(object sEnder, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == "Time")
        {
            StartTimePicker.Time = StartTimeIn;
            CheckSetEndDateTime();
        }
    }

    private void GtkStartTime_SelectedIndexChanged(object sender, EventArgs e)
    {
        StartTimeIn = new TimeSpan(GtkStartHourPicker.SelectedIndex, GtkStartMinutsPicker.SelectedIndex, 0);
        CheckSetEndDateTime();
    }

    private void StartTimePicker_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == "Time")
        {
            StartTimeIn = StartTimePicker.Time.GetValueOrDefault();
            CheckSetEndDateTime();
        }
    }

    [RelayCommand]
    public void StartDateTimeNowButtonClicked()
    {
        StartDateIn = DateTime.Now.Date;
        StartTimeIn = DateTime.Now.TimeOfDay;
        SetStartDateTime();
        CheckSetEndDateTime();
    }


    //FROM HERE Combined

    private void OnCombinedEntryFocused(object sender, FocusEventArgs e) => ClearTotYMWDHM(null);
    private void OnCombinedEntryUnfocused(object sender, FocusEventArgs e) => OnCombinedEntryCompleted(sender, e);

    private async void OnCombinedEntryCompleted(object sEnder, EventArgs args)
    {
        Entry TheEntry = (Entry)sEnder;
        if (!int.TryParse(TheEntry.Text, out int result) && TheEntry.Text.Length != 0)
        {
            DictionaryOfCombinedEntries[TheEntry] = 0;
            var TextHolder = TheEntry.Text;
            TheEntry.Text = "";
			await DisplayAlertAsync("Invalid \"Combined Years\" ", TextHolder, "OK");
            TheEntry.Focus();
        }
        else
        {
            DictionaryOfCombinedEntries[TheEntry] = result;
        }
    }

    //TO HERE Combined


    //FROM HERE Total

    private void OnTotEntryFocused(object sender, FocusEventArgs e) => ClearYMWDHM((Entry)sender);
    private void OnTotEntryUnfocused(object sender, FocusEventArgs e) => OnTotEntryCompleted(sender, e);

    private async void OnTotEntryCompleted(object sEnder, EventArgs args)
    {
        Entry TheEntry = (Entry)sEnder;
        if (!int.TryParse(TheEntry.Text, out int result) && TheEntry.Text.Length != 0)
        {
            DictionaryOfTotalEntries[TheEntry] = 0;
            var TextHolder = TheEntry.Text;
            TheEntry.Text = "";
			await DisplayAlertAsync("Invalid \"Total Years\" ", TextHolder, "OK");
            TheEntry.Focus();
        }
        else
        {
            DictionaryOfTotalEntries[TheEntry] = result;
        }
    }

    //TO HERE Total


    // End date-time... 
    [RelayCommand]
    private void CalcEndTimeBtnClicked()
    {
        DoCalcStartTime = false;
        DoCalcEndTime = true;
        DoCalcYMWDHM = false;
        LabelEqual.Text = "=";
        LabelPlus.Text = "+";
        DoCalculate();
    }


    private void CheckSetStartDateTime()
    {
        if (StartDateIn > EndDateIn)
        {
            StartDateIn = EndDateIn;
            StartTimeIn = EndTimeIn;
            SetStartDateTime();
        }
        else if (StartDateIn == EndDateIn && StartTimeIn > EndTimeIn)
        {
            StartTimeIn = EndTimeIn;
            SetStartDateTime();
        }
    }

    private void EndDatePicker_DateSelected(object sender, DateChangedEventArgs e)
    {
        EndDateIn = e.NewDate.GetValueOrDefault();
        MacEndDatePicker.Date = EndDateIn;
        EndDayName.Text = EndDateIn.DayOfWeek.ToString().Remove(3);
        CheckSetStartDateTime();
    }

    private void OnMacEndDatePickerDateSelected(object sEnder, DateChangedEventArgs e)
    {
        EndDateIn = e.NewDate.GetValueOrDefault();
        EndDatePicker.Date = EndDateIn;
        EndDayName.Text = EndDateIn.DayOfWeek.ToString().Remove(3);
        CheckSetStartDateTime();
    }

    private void OnMacEndTimePickerPropertyChanged(object sEnder, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == "Time")
        {
            EndTimePicker.Time = EndTimeIn;
            CheckSetStartDateTime();
        }
    }

    private void GtkEndTime_SelectedIndexChanged(object sender, EventArgs e)
    {
        EndTimeIn = new TimeSpan(GtkEndHourPicker.SelectedIndex, GtkEndMinutsPicker.SelectedIndex, 0);
        CheckSetStartDateTime();
    }

    private void EndTimePicker_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == "Time")
        {
            EndTimeIn = EndTimePicker.Time.GetValueOrDefault();
            CheckSetStartDateTime();
        }
    }

    [RelayCommand]
    private void EndDateTimeNowButtonClicked()
    {
        EndDateIn = DateTime.Now.Date;
        EndTimeIn = DateTime.Now.TimeOfDay;
        SetEndDateTime();
        CheckSetStartDateTime();
    }

    [RelayCommand]
    private void ClearAllButtonClicked() => DoClearAll();

    private async void CalcAndShowTimeSpans()
    {
        CombndYearsOut = EndDateTimeIn.Year - StartDateTimeIn.Year;
        CombndMonthsOut = EndDateTimeIn.Month - StartDateTimeIn.Month;
        if (CombndMonthsOut < 0)
        {
            CombndMonthsOut += 12;
            CombndYearsOut--;
        }
        DateTime dtCalc1 = StartDateTimeIn.AddYears(CombndYearsOut).AddMonths(CombndMonthsOut);
        TimeSpan ts1 = dtCalc1 - StartDateTimeIn; // Days in years + months
        TimeSpan ts2 = EndDateTimeIn - StartDateTimeIn; // Whole span
        CombndDaysOut = ts2.Days - ts1.Days;
        if (CombndDaysOut < 0)
        {
            CombndMonthsOut--;
            if (CombndMonthsOut < 0)
            {
                CombndMonthsOut += 12;
                CombndYearsOut--;
            }
            DateTime dtCalc2 = StartDateTimeIn.AddYears(CombndYearsOut).AddMonths(CombndMonthsOut);
            CombndDaysOut = (ts2 - (dtCalc2 - StartDateTimeIn)).Days;
        }
        CombndHoursOut = ts2.Hours;
        CombndMinutesOut = ts2.Minutes;
        CombndWeeksOut = CombndDaysOut / 7;
        CombndDaysOut %= 7;
        TotDaysOut = (long)ts2.TotalDays;
        TotWeeksOut = TotDaysOut / 7;
        TotMonthsOut = CombndMonthsOut + 12 * CombndYearsOut;
        TotYearsOut = CombndYearsOut;
        TotHoursOut = (long)ts2.TotalHours;
        TotMinutesOut = (long)ts2.TotalMinutes;

        MainThread.BeginInvokeOnMainThread(() =>
        {
            CombndDays.Text = CombndDaysOut.ToString();
            CombndWeeks.Text = CombndWeeksOut.ToString();
            CombndMonths.Text = CombndMonthsOut.ToString();
            CombndYears.Text = CombndYearsOut.ToString();
            CombndHours.Text = CombndHoursOut.ToString();
            CombndMinutes.Text = CombndMinutesOut.ToString();
            TotDays.Text = TotDaysOut.ToString();
            TotWeeks.Text = TotWeeksOut.ToString();
            TotMonths.Text = TotMonthsOut.ToString();
            TotYears.Text = TotYearsOut.ToString();
            TotHours.Text = TotHoursOut.ToString();
            TotMinutes.Text = TotMinutesOut.ToString();
        });

		if (TotDaysOut > 9999999999) await DisplayAlertAsync("Total \"Days\" > 9999999999", TotDays.ToString(), "OK");
		if (TotHoursOut > 9999999999) await DisplayAlertAsync("Total \"Hours\" > 9999999999", TotHours.ToString(), "OK");
		if (TotMinutesOut > 9999999999) await DisplayAlertAsync("Total \"Minutes\" > 9999999999", TotMinutes.ToString(), "OK");
    }

    private void OnCalculateButtonClicked(object sEnder, EventArgs e) => DoCalculate();

    enum EntryNames : int { years = 0, months = 1, weeks = 2, days = 3, hours = 4, minutes = 5 }

    private async void DoCalculate()
    {
        StartTimeIn = StartTimePicker.Time.GetValueOrDefault();
        EndTimeIn = EndTimePicker.Time.GetValueOrDefault();
        StartDateIn = StartDatePicker.Date.GetValueOrDefault();
        EndDateIn = EndDatePicker.Date.GetValueOrDefault();
        EndDateTimeIn = EndDateIn + EndTimeIn;
        StartDateTimeIn = StartDateIn + StartTimeIn;
        StartDateTimeOut = DateTime.MaxValue;
        EndDateTimeOut = DateTime.MaxValue;

        if (DoCalcYMWDHM)
        {
            CalcAndShowTimeSpans();
        }
        else
        {
            foreach (Entry CurEntry in DictionaryOfCombinedEntries.Keys)
            {
                if (!int.TryParse(CurEntry.Text, out int result) && CurEntry.Text.Length != 0)
                {
                    DictionaryOfCombinedEntries[CurEntry] = 0;
                    string TextHolder = CurEntry.Text;
                    CurEntry.Text = "";
					await DisplayAlertAsync("Invalid \"Combined Value\" ", TextHolder, "OK");
                    CurEntry.Focus();
                    return;
                }
                DictionaryOfCombinedEntries[CurEntry] = result;
            }
            foreach (Entry CurEntry in DictionaryOfTotalEntries.Keys)
            {
                if (!int.TryParse(CurEntry.Text, out int result) && CurEntry.Text.Length != 0)
                {
                    DictionaryOfTotalEntries[CurEntry] = 0;
                    string TextHolder = CurEntry.Text;
                    CurEntry.Text = "";
					await DisplayAlertAsync("Invalid \"Total Value\" ", TextHolder, "OK");
                    CurEntry.Focus();
                    return;
                }
                DictionaryOfTotalEntries[CurEntry] = result;
            }
        }

        bool TotalsAllZero = DictionaryOfTotalEntries.Values.All(v => v == 0);
        bool CombinedsAllZero = DictionaryOfCombinedEntries.Values.All(v => v == 0);

        if (DoCalcEndTime)
        {
            if (!TotalsAllZero || !CombinedsAllZero)
            {
                if (TotalsAllZero ^ CombinedsAllZero)
                {
                    EndDateTimeOut = DateTime.MaxValue;
                    if (!TotalsAllZero)
                    {
                        for (int i = 0; i < DictionaryOfTotalEntries.Count; i++)
                        {
                            if (DictionaryOfTotalEntries.ElementAt(i).Value != 0)
                            {
                                bool RestIsZero = DictionaryOfTotalEntries.Skip(i + 1).All(kv => kv.Value == 0);
                                if (RestIsZero)
                                {
                                    try
                                    {
                                        switch (i)
                                        {
                                            case (int)EntryNames.years: EndDateTimeOut = StartDateTimeIn.AddYears(DictionaryOfTotalEntries.ElementAt(i).Value); break;
                                            case (int)EntryNames.months: EndDateTimeOut = StartDateTimeIn.AddMonths(DictionaryOfTotalEntries.ElementAt(i).Value); break;
                                            case (int)EntryNames.weeks: EndDateTimeOut = StartDateTimeIn.AddDays(DictionaryOfTotalEntries.ElementAt(i).Value * 7); break;
                                            case (int)EntryNames.days: EndDateTimeOut = StartDateTimeIn.AddDays(DictionaryOfTotalEntries.ElementAt(i).Value); break;
                                            case (int)EntryNames.hours: EndDateTimeOut = StartDateTimeIn.AddHours(DictionaryOfTotalEntries.ElementAt(i).Value); break;
                                            case (int)EntryNames.minutes: EndDateTimeOut = StartDateTimeIn.AddMinutes(DictionaryOfTotalEntries.ElementAt(i).Value); break;
                                        }
                                    }
                                    catch (ArgumentOutOfRangeException outOfRange)
                                    {
				await DisplayAlertAsync("Argument Out Of Range", outOfRange.Message.Remove(outOfRange.Message.IndexOf(" name:")) + ": value = " + DictionaryOfTotalEntries.ElementAt(i).Value + "\nMax DateTime is " + DateTime.MaxValue.ToString("u").Remove(16), "OK");
                                        DictionaryOfTotalEntries[DictionaryOfTotalEntries.ElementAt(i).Key] = 0;
                                        DictionaryOfTotalEntries.ElementAt(i).Key.Text = "";
                                        DictionaryOfTotalEntries.ElementAt(i).Key.Focus();
                                        return;
                                    }
                                }
                                else
                                {
										await DisplayAlertAsync("Type error", "Only one \"Total\" value allowed", "OK");
                                }
                            }
                        }
                    }
                    else // Combined
                    {
                        EndDateTimeOut = StartDateTimeIn;
                        int i = 0;
                        foreach (var kv in DictionaryOfCombinedEntries)
                        {
                            if (kv.Value != 0)
                            {
                                try
                                {
                                    switch (i)
                                    {
                                        case (int)EntryNames.years: EndDateTimeOut = EndDateTimeOut.AddYears(kv.Value); break;
                                        case (int)EntryNames.months: EndDateTimeOut = EndDateTimeOut.AddMonths(kv.Value); break;
                                        case (int)EntryNames.weeks: EndDateTimeOut = EndDateTimeOut.AddDays(kv.Value * 7); break;
                                        case (int)EntryNames.days: EndDateTimeOut = EndDateTimeOut.AddDays(kv.Value); break;
                                        case (int)EntryNames.hours: EndDateTimeOut = EndDateTimeOut.AddHours(kv.Value); break;
                                        case (int)EntryNames.minutes: EndDateTimeOut = EndDateTimeOut.AddMinutes(kv.Value); break;
                                    }
                                }
                                catch (ArgumentOutOfRangeException outOfRange)
                                {
								await DisplayAlertAsync("Argument Out Of Range", outOfRange.Message.Remove(outOfRange.Message.IndexOf(" name:")) + ": combined value = " + kv.Value + "\nMax DateTime is " + DateTime.MaxValue.ToString("u").Remove(16), "OK");
                                    DictionaryOfCombinedEntries[kv.Key] = 0;
                                    kv.Key.Text = "";
                                    kv.Key.Focus();
                                    return;
                                }
                            }
                            i++;
                        }
                    }

                    if (EndDateTimeOut != DateTime.MaxValue)
                    {
                        var tmpFlag = DoCalcEndTime;
                        DoClearAll();
                        DoCalcEndTime = tmpFlag;
                        EndDateTimeIn = EndDateTimeOut;
                        EndDateIn = EndDateTimeOut.Date;
                        EndTimeIn = EndDateTimeOut.TimeOfDay;
                        SetEndDateTime();
                        CalcAndShowTimeSpans();
                    }
                }
                else
                {
					await DisplayAlertAsync("Type error", "Not both \"Total\" and \"Combined\" time spans can be used", "OK");
                }
            }
            else
            {
                EndDateTimeOut = StartDateTimeIn; var tmpFlag = DoCalcEndTime; DoClearAll(); DoCalcEndTime = tmpFlag; EndDateTimeIn = EndDateTimeOut; EndDateIn = EndDateTimeOut.Date; EndTimeIn = EndDateTimeOut.TimeOfDay; SetEndDateTime(); CalcAndShowTimeSpans();
            }
        }

        if (DoCalcStartTime)
        {
            if (DoCalcEndTime)
            {
				await DisplayAlertAsync("Error", "Can't calculate both \"Start\" and \"End\"", "OK");
                return;
            }
            if (!TotalsAllZero || !CombinedsAllZero)
            {
                if (TotalsAllZero ^ CombinedsAllZero)
                {
                    StartDateTimeOut = DateTime.MaxValue;
                    if (!TotalsAllZero)
                    {
                        for (int i = 0; i < DictionaryOfTotalEntries.Count; i++)
                        {
                            if (DictionaryOfTotalEntries.ElementAt(i).Value != 0)
                            {
                                bool RestIsZero = DictionaryOfTotalEntries.Skip(i + 1).All(kv => kv.Value == 0);
                                if (RestIsZero)
                                {
                                    try
                                    {
                                        switch (i)
                                        {
                                            case (int)EntryNames.years: StartDateTimeOut = EndDateTimeIn.AddYears(-DictionaryOfTotalEntries.ElementAt(i).Value); break;
                                            case (int)EntryNames.months: StartDateTimeOut = EndDateTimeIn.AddMonths(-DictionaryOfTotalEntries.ElementAt(i).Value); break;
                                            case (int)EntryNames.weeks: StartDateTimeOut = EndDateTimeIn.AddDays(-DictionaryOfTotalEntries.ElementAt(i).Value * 7); break;
                                            case (int)EntryNames.days: StartDateTimeOut = EndDateTimeIn.AddDays(-DictionaryOfTotalEntries.ElementAt(i).Value); break;
                                            case (int)EntryNames.hours: StartDateTimeOut = EndDateTimeIn.AddHours(-DictionaryOfTotalEntries.ElementAt(i).Value); break;
                                            case (int)EntryNames.minutes: StartDateTimeOut = EndDateTimeIn.AddMinutes(-DictionaryOfTotalEntries.ElementAt(i).Value); break;
                                        }
                                    }
                                    catch (ArgumentOutOfRangeException outOfRange)
                                    {
										await DisplayAlertAsync("Argument Out Of Range", outOfRange.Message.Remove(outOfRange.Message.IndexOf(" name:")) + ": value = " + DictionaryOfTotalEntries.ElementAt(i).Value + "\nMax DateTime is " + DateTime.MaxValue.ToString("u").Remove(16), "OK");
                                        DictionaryOfTotalEntries[DictionaryOfTotalEntries.ElementAt(i).Key] = 0;
                                        DictionaryOfTotalEntries.ElementAt(i).Key.Text = "";
                                        DictionaryOfTotalEntries.ElementAt(i).Key.Focus();
                                        return;
                                    }
                                }
                                else
                                {
										await DisplayAlertAsync("Type error", "Only one \"Total\" value allowed", "OK");
                                }
                            }
                        }
                    }
                    else // Combined
                    {
                        StartDateTimeOut = EndDateTimeIn;
                        int i = 0;
                        foreach (var kv in DictionaryOfCombinedEntries)
                        {
                            if (kv.Value != 0)
                            {
                                try
                                {
                                    switch (i)
                                    {
                                        case (int)EntryNames.years: StartDateTimeOut = StartDateTimeOut.AddYears(-kv.Value); break;
                                        case (int)EntryNames.months: StartDateTimeOut = StartDateTimeOut.AddMonths(-kv.Value); break;
                                        case (int)EntryNames.weeks: StartDateTimeOut = StartDateTimeOut.AddDays(-kv.Value * 7); break;
                                        case (int)EntryNames.days: StartDateTimeOut = StartDateTimeOut.AddDays(-kv.Value); break;
                                        case (int)EntryNames.hours: StartDateTimeOut = StartDateTimeOut.AddHours(-kv.Value); break;
                                        case (int)EntryNames.minutes: StartDateTimeOut = StartDateTimeOut.AddMinutes(-kv.Value); break;
                                    }
                                }
                                catch (ArgumentOutOfRangeException outOfRange)
                                {
								await DisplayAlertAsync("Argument Out Of Range", outOfRange.Message.Remove(outOfRange.Message.IndexOf(" name:")) + ": combined value = " + kv.Value + "\nMax DateTime is " + DateTime.MaxValue.ToString("u").Remove(16), "OK");
                                    DictionaryOfCombinedEntries[kv.Key] = 0; kv.Key.Text = ""; kv.Key.Focus(); return;
                                }
                            }
                            i++;
                        }
                    }
                    if (StartDateTimeOut != DateTime.MaxValue)
                    {
                        var tmpFlag = DoCalcStartTime; DoClearAll(); DoCalcStartTime = tmpFlag; StartDateTimeIn = StartDateTimeOut; StartDateIn = StartDateTimeOut.Date; StartTimeIn = StartDateTimeOut.TimeOfDay; SetStartDateTime(); CalcAndShowTimeSpans();
                    }
                }
                else
                {
					await DisplayAlertAsync("Type error", "Not both \"Total\" and \"Combined\" time spans can be used", "OK");
                }
            }
            else
            {
                StartDateTimeOut = EndDateTimeIn; var tmpFlag = DoCalcStartTime; DoClearAll(); DoCalcStartTime = tmpFlag; StartDateTimeIn = StartDateTimeOut; StartDateIn = StartDateTimeOut.Date; StartTimeIn = StartDateTimeOut.TimeOfDay; SetStartDateTime(); CalcAndShowTimeSpans();
            }
        }
    }

    [RelayCommand]
    private async Task HelpButtonClicked() => await Shell.Current.GoToAsync(nameof(AboutHelp), true);

    [RelayCommand]
    private void CalcYMWDHMBtnClicked()
    {
        DoCalcStartTime = false;
        DoCalcEndTime = false;
        DoCalcYMWDHM = true;
        LabelEqual.Text = "=";
        LabelPlus.Text = "+";
        DoCalculate();
    }

    private string CalendarItem = "";
    private bool CorrectForIcsTimeZone = false;
    private readonly string filetypeToReadFrom = "ics";

    private async void On_OpenIcsMessageReceived(object recipient, OpenIcsMessageArgs message)
    {
        CorrectForIcsTimeZone = message.CorrectForTimeZone;
        var selectedFiles = await OLD_FileHandler.SelectFiles(filetypeToReadFrom);
        if (selectedFiles != null)
            On_FileToReadFromSelectedAsync(selectedFiles);
    }

    private async void On_FileToReadFromSelectedAsync(SelectFilesResult arg2)
    {
        if (arg2.DidPick && arg2.pickResult != null)
        {
            List<string> TheIcsTxt = new();
            try
            {
                using StreamReader sr = new(await arg2.pickResult.OpenReadAsync());
                string? line;
                while ((line = sr.ReadLine()) != null)
                    TheIcsTxt.Add(line);
            }
            catch (Exception e)
            {
				await DisplayAlertAsync("The file could not be read:", e.Message, "OK");
            }

            try
            {
                var IdxBEGIN_STANDARD = TheIcsTxt.FindIndex(s => s.Contains(@"BEGIN:STANDARD"));
                var IdxEND_STANDARD = TheIcsTxt.FindIndex(s => s.Contains(@"END:STANDARD"));
                var LgthSTANDARD = IdxEND_STANDARD - IdxBEGIN_STANDARD;
                var TimeIDX = TheIcsTxt.FindIndex(IdxBEGIN_STANDARD, LgthSTANDARD, s => s.Contains(@"TZOFFSETTO:"));
                int SignIdx = TheIcsTxt[TimeIDX].IndexOfAny("+-".ToCharArray(), TheIcsTxt[TimeIDX].LastIndexOf(':'));
                var TheSign = TheIcsTxt[TimeIDX][SignIdx];
                var StartOfTimeStringIDX = ++SignIdx;
                var LgthOfTimestring = TheIcsTxt[TimeIDX].Length - StartOfTimeStringIDX;
                var TimeString = TheIcsTxt[TimeIDX].Substring(StartOfTimeStringIDX, LgthOfTimestring);
                var TheTZOFFSETTO = TimeSpan.ParseExact(TimeString, "hhmm", null);
                if (TheSign == '-') TheTZOFFSETTO = TimeSpan.Zero - TheTZOFFSETTO;
                var BaseUtcOff = TimeZoneInfo.Local.BaseUtcOffset;
                TimeIDX = TheIcsTxt.FindIndex(s => s.Contains(@"DTSTART;TZID="));
                StartOfTimeStringIDX = TheIcsTxt[TimeIDX].LastIndexOf(':') + 1;
                LgthOfTimestring = TheIcsTxt[TimeIDX].Length - StartOfTimeStringIDX;
                TimeString = TheIcsTxt[TimeIDX].Substring(StartOfTimeStringIDX, LgthOfTimestring);
                StartDateTimeOut = DateTime.ParseExact(TimeString, @"yyyyMMddTHHmm00", null);
                if (CorrectForIcsTimeZone) { StartDateTimeOut -= TheTZOFFSETTO; StartDateTimeOut += BaseUtcOff; }
                StartDateTimeIn = StartDateTimeOut; StartDateIn = StartDateTimeOut.Date; StartTimeIn = StartDateTimeOut.TimeOfDay; SetStartDateTime();
                TimeIDX = TheIcsTxt.FindIndex(s => s.Contains(@"DTEND;TZID="));
                StartOfTimeStringIDX = TheIcsTxt[TimeIDX].LastIndexOf(':') + 1;
                LgthOfTimestring = TheIcsTxt[TimeIDX].Length - StartOfTimeStringIDX;
                TimeString = TheIcsTxt[TimeIDX].Substring(StartOfTimeStringIDX, LgthOfTimestring);
                EndDateTimeOut = DateTime.ParseExact(TimeString, @"yyyyMMddTHHmm00", null);
                if (CorrectForIcsTimeZone) { EndDateTimeOut -= TheTZOFFSETTO; EndDateTimeOut += BaseUtcOff; }
                EndDateTimeIn = EndDateTimeOut; EndDateIn = EndDateTimeOut.Date; EndTimeIn = EndDateTimeOut.TimeOfDay; SetEndDateTime();
                CalcAndShowTimeSpans();

				// Ensure the phone/tablet layout is applied after returning from the picker without clearing the loaded values.
				var info = DeviceDisplay.Current.MainDisplayInfo;
				ApplyOrientationAndZoom(info.Orientation, clearAll: false);
            }
            catch (Exception e)
            {
				await DisplayAlertAsync("Bad .ics file", e.Message, "OK");
            }

            await Shell.Current.GoToAsync("..\\..", true);
        }
    }

    private string SuggestedNameOfFileToSaveTo = "";
    private async void On_SaveToIcsMessageReceived(object recipient, SaveToIcsMessageArgs message)
    {
        DateTime DateStart = StartDateIn + StartTimeIn;
        DateTime DateEnd = EndDateIn + EndTimeIn;
        string Summary = message.EventName_Summary;
        string Location = message.Location;
        string Description = message.TheDescription;
        StringBuilder sb = new();
        sb.AppendLine("BEGIN:VCALENDAR");
        sb.AppendLine("VERSION:2.0");
        sb.AppendLine("PRODID:eksit.dk");
        sb.AppendLine("METHOD:PUBLISH");
        var TimeZoneName = TimeZoneInfo.Local.StandardName;
        var UtcOffset = TimeZoneInfo.Local.GetUtcOffset(DateTime.Now);
        var UtcOffsetStr = (UtcOffset.Hours >= 0 ? "+" : "-") + UtcOffset.ToString("hhmm");
        var BaseUtcOff = TimeZoneInfo.Local.BaseUtcOffset;
        var BaseUtcOffStr = (BaseUtcOff.Hours >= 0 ? "+" : "-") + BaseUtcOff.ToString("hhmm");
        sb.AppendLine("BEGIN:VTIMEZONE");
        sb.AppendLine("TZID:" + TimeZoneName);
        sb.AppendLine("BEGIN:STANDARD");
        sb.AppendLine("TZOFFSETFROM:" + UtcOffsetStr);
        sb.AppendLine("TZOFFSETTO:" + BaseUtcOffStr);
        sb.AppendLine("END:STANDARD");
        sb.AppendLine("BEGIN:DAYLIGHT");
        sb.AppendLine("TZOFFSETFROM:" + BaseUtcOffStr);
        sb.AppendLine("TZOFFSETTO:" + UtcOffsetStr);
        sb.AppendLine("END:DAYLIGHT");
        sb.AppendLine("END:VTIMEZONE");
        sb.AppendLine("BEGIN:VEVENT");
        sb.AppendLine("DTSTART;TZID=\"" + TimeZoneName + "\":" + DateStart.ToString("yyyyMMddTHHmm00"));
        sb.AppendLine("DTEND;TZID=\"" + TimeZoneName + "\":" + DateEnd.ToString("yyyyMMddTHHmm00"));
        sb.AppendLine("SUMMARY:" + Summary);
        sb.AppendLine("LOCATION:" + Location);
        sb.AppendLine("DESCRIPTION:" + Description);
        sb.AppendLine("PRIORITY:5");
        sb.AppendLine("END:VEVENT");
        sb.AppendLine("END:VCALENDAR");
        CalendarItem = sb.ToString().Replace("\r", "");
        SuggestedNameOfFileToSaveTo = Summary;
        var suggestedFileName = string.IsNullOrWhiteSpace(SuggestedNameOfFileToSaveTo) ? "Calendar" : SuggestedNameOfFileToSaveTo;
        foreach (var invalid in Path.GetInvalidFileNameChars())
            suggestedFileName = suggestedFileName.Replace(invalid, '_');
        var finalFileName = $"{suggestedFileName}.ics";

#if ANDROID
        var filePath = Path.Combine(FileSystem.Current.CacheDirectory, finalFileName);
        await File.WriteAllTextAsync(filePath, CalendarItem, Encoding.UTF8);
        await Share.Default.RequestAsync(new ShareFileRequest
        {
            Title = "Save .ics file",
            File = new ShareFile(filePath)
        });
        await Shell.Current.GoToAsync("..\\..", true);
#else
        using MemoryStream stream = new(Encoding.UTF8.GetBytes(CalendarItem));
        FileSaverResult fileSaverResult = await FileSaver.Default.SaveAsync(finalFileName, stream);
        if (fileSaverResult.IsSuccessful)
            await Shell.Current.GoToAsync("..\\..", true);
        else
            await Shell.Current.DisplayAlertAsync("Error", "File is not saved!!\n\n" + fileSaverResult.Exception, "OK");
#endif
    }

    [RelayCommand]
    private async Task FileButton_Clicked() => await Shell.Current.GoToAsync(nameof(FileICS), true);

    [RelayCommand]
    private async Task SettingsButtonClicked()
    {
        if (!Ui.ZoomSettings.IsZoomSupportedForCurrentDevice)
            return;

        await Shell.Current.GoToAsync(nameof(SettingsPage), true);
    }

    private async void On_FileToSaveToSelected(SelectFilesResult arg2)
    {
        if (arg2.DidPick && arg2.pickResult != null)
        {
            using MemoryStream stream = new(Encoding.Default.GetBytes(CalendarItem));
            FileSaverResult? fileSaveResult = await OLD_FileHandler.SaveToTextFile(stream, arg2.pickResult.FullPath);
            stream.Dispose();
        }
    }
}
