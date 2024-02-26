using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ChronoWiz.FileHandlers;

namespace ChronoWiz.View;

// Learn more about making custom code visible in the Xamarin.Forms previewer
// by visiting https://aka.ms/xamarinforms-previewer
[DesignTimeVisible(true)]
public partial class MainPage : ContentPage
{
	public MainPage()
	{
#if DEBUG
		try
		{
			InitializeComponent();
		}
		catch (Exception ex)
		{
			var tst = ex;
		}
#else
			InitializeComponent();
#endif
		BindingContext = this;

		WeakReferenceMessenger.Default.Register<SaveToIcsMessageArgs, string>
			(this, MessengerKeys.SaveToIcsMessageKey, On_SaveToIcsMessageReceived);

		WeakReferenceMessenger.Default.Register<OpenIcsMessageArgs, string>
			(this, MessengerKeys.OpenIcsMessageKey, On_OpenIcsMessageReceived);


		Resources["DynamicBaseButtonStyle"] = Resources["baseButtonStyle"];

		DictionaryOfCombinedEntries = new Dictionary<Entry, int>()
		{
			{ CombndYears,      0 }
			,
			{ CombndMonths,     0 }
			,
			{ CombndWeeks,      0 }
			,
			{ CombndDays,       0 }
			,
			{ CombndHours,      0 }
			,
			{ CombndMinutes,    0 }
		};
		DictionaryOfTotalEntries = new Dictionary<Entry, int>()
		{
			{ TotYears,     0 }
			,
			{ TotMonths,    0 }
			,
			{ TotWeeks,     0 }
			,
			{ TotDays,      0 }
			,
			{ TotHours,     0 }
			,
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


#if __MACCATALYST__
		SetOrientationRight
		(
			DeviceDisplay.Current.MainDisplayInfo.Width
				,
			DeviceDisplay.Current.MainDisplayInfo.Height
				,
			DisplayOrientation.Landscape
		);
#else
		SetOrientationRight
		(
			DeviceDisplay.Current.MainDisplayInfo.Width
				,
			DeviceDisplay.Current.MainDisplayInfo.Height
				,
			DeviceDisplay.Current.MainDisplayInfo.Orientation
		);
#endif
		DeviceDisplay.Current.MainDisplayInfoChanged += Current_MainDisplayInfoChanged;
	}

	private void Current_MainDisplayInfoChanged(object sender, DisplayInfoChangedEventArgs e)
	{
		SetOrientationRight
		(
			e.DisplayInfo.Width
				,
			e.DisplayInfo.Height
				,
			e.DisplayInfo.Orientation
		);
	}

	private void SetOrientationRight
	(
		double DipsWidth
			,
		double DipsHight
			,
		DisplayOrientation DipsOrient
	)
	{
		bool portrait = (DipsOrient == DisplayOrientation.Portrait);

		TotalStackName.TranslationX = 0.0f;
		TotalStackName.TranslationY = 0.0f;


		if (firstTimeWdthOrHeightChanged)
		{
			StartDateTimeIntroLabelNameFontSizeOrig = StartDateTimeIntroLabelName.FontSize;
			StartEndDayNameFontSizeOrig = StartDayName.FontSize;
			firstTimeWdthOrHeightChanged = false;
		}

		if (portrait)
		{ // Portrait
			EntriesCenterOuterStack.Orientation = StackOrientation.Horizontal;
			EntriesCenterCombndStack.Orientation = StackOrientation.Vertical;
			EntriesCenterTotStack.Orientation = StackOrientation.Vertical;
		}
		else
		{ // Landscape
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
			if (portrait) // Portrait ?
			{ // Portrait
				StartLabelNDateTimeStack.Orientation = StackOrientation.Vertical;
				EndLabelNDateTimeStack.Orientation = StackOrientation.Vertical;
			}
			else
			{ // Landscape
				StartLabelNDateTimeStack.Orientation = StackOrientation.Horizontal;
				EndLabelNDateTimeStack.Orientation = StackOrientation.Horizontal;
			}

			StartDayName.WidthRequest = EndDayName.WidthRequest = 50;

		}
		else if (DeviceInfo.Platform == DevicePlatform.iOS)
		{
			if (portrait) // Portrait ?
			{ // Portrait
				StartLabelNDateTimeStack.Orientation = StackOrientation.Vertical;
				EndLabelNDateTimeStack.Orientation = StackOrientation.Vertical;
			}
			else
			{ // Landscape
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

		DoClearAll();
	}

	protected override void OnSizeAllocated(double width, double height)
	{
		base.OnSizeAllocated(width, height);

		TotalStackName.Scale = 1.0f / TotalStackName.Scale;

		double WidthFactor = width / TotalStackName.Width;
		double HeightFactor = height / TotalStackName.Height;

		if (WidthFactor < HeightFactor)
		{
			TotalStackName.Scale = WidthFactor;
		}
		else
		{
			TotalStackName.Scale = HeightFactor;
		}
	}

	private bool firstTimeWdthOrHeightChanged = true;

	double nativeTotalStackHeightPortrait = 732.0;


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
		get { return PrivEnteredYMWDHMTimeSpan; }
		set => PrivEnteredYMWDHMTimeSpan = value;
	}

	// Values for "Combined" dateTime span
	private Dictionary<Entry, int> DictionaryOfCombinedEntries;

	// Total values for dateTime span
	public Dictionary<Entry, int> DictionaryOfTotalEntries;


	// Output values
	// Combnd
	private int CombndYearsOut = 0;
	private int CombndMonthsOut = 0;
	private int CombndWeeksOut = 0;
	private int CombndDaysOut = 0;
	private int CombndHoursOut = 0;
	private int CombndMinutesOut = 0;
	// Total values for dateTime span
	private Int64 TotYearsOut = 0;
	private Int64 TotMonthsOut = 0;
	private Int64 TotWeeksOut = 0;
	private Int64 TotDaysOut = 0;
	private Int64 TotHoursOut = 0;
	private Int64 TotMinutesOut = 0;


	private void SetStartDateTime()
	{
		try
		{
			// TODO Xamarin.Forms.Device.RuntimePlatform is no longer supported. Use Microsoft.Maui.Devices.DeviceInfo.Platform instead. For more details see https://learn.microsoft.com/en-us/dotnet/maui/migration/forms-projects#device-changes
			//if (DeviceInfo.Platform == DevicePlatform.GTK)
			//{
			//	// Remove event
			//	GtkStartHourPicker.SelectedIndexChanged -= GtkStartTime_SelectedIndexChanged;
			//	GtkStartMinutsPicker.SelectedIndexChanged -= GtkStartTime_SelectedIndexChanged;
			//	// Show time
			//	GtkStartHourPicker.SelectedIndex = StartTimeIn.Hours;
			//	GtkStartMinutsPicker.SelectedIndex = StartTimeIn.Minutes;
			//	// Restore event
			//	GtkStartHourPicker.SelectedIndexChanged += GtkStartTime_SelectedIndexChanged;
			//	GtkStartMinutsPicker.SelectedIndexChanged += GtkStartTime_SelectedIndexChanged;
			//}
			//else
			//{
			//MacStartDatePicker.Date = StartDateIn;
			//MacStartTimePicker.Time = new TimeSpan(StartTimeIn.Hours, StartTimeIn.Minutes, 0);

			StartTimePicker.Time = new TimeSpan(StartTimeIn.Hours, StartTimeIn.Minutes, 0);
			//}

			StartDatePicker.Date = StartDateIn;

			StartDayName.Text = StartDateIn.DayOfWeek.ToString().Remove(3);

		}
		catch (Exception)
		{
		}
	}

	private void SetEndDateTime()
	{
		try
		{
			// TODO Xamarin.Forms.Device.RuntimePlatform is no longer supported. Use Microsoft.Maui.Devices.DeviceInfo.Platform instead. For more details see https://learn.microsoft.com/en-us/dotnet/maui/migration/forms-projects#device-changes
			//if (DeviceInfo.Platform == DevicePlatform.GTK)
			//{
			//	// Remove events
			//	GtkEndHourPicker.SelectedIndexChanged -= GtkEndTime_SelectedIndexChanged;
			//	GtkEndMinutsPicker.SelectedIndexChanged -= GtkEndTime_SelectedIndexChanged;
			//	// Show time
			//	GtkEndHourPicker.SelectedIndex = EndTimeIn.Hours;
			//	GtkEndMinutsPicker.SelectedIndex = EndTimeIn.Minutes;
			//	// Restore events
			//	GtkEndHourPicker.SelectedIndexChanged += GtkEndTime_SelectedIndexChanged;
			//	GtkEndMinutsPicker.SelectedIndexChanged += GtkEndTime_SelectedIndexChanged;
			//}
			//else
			//{
			//MacEndDatePicker.Date = EndDateIn;
			//MacEndTimePicker.Time = new TimeSpan(EndTimeIn.Hours, EndTimeIn.Minutes, 0);

			EndTimePicker.Time = new TimeSpan(EndTimeIn.Hours, EndTimeIn.Minutes, 0);
			//}

			EndDatePicker.Date = EndDateIn;

			EndDayName.Text = EndDateIn.DayOfWeek.ToString().Remove(3);

		}
		catch (Exception)
		{
		}
	}

	private void ClearTotIOVars(Entry ImInFocus)
	{
		// Total values for dateTime span
		foreach (Entry entry in DictionaryOfTotalEntries.Keys)
		{
			if (entry != ImInFocus)
			{
				DictionaryOfTotalEntries[entry] = 0;
			}
		}
		// Total values for dateTime span
		TotYearsOut = 0;
		TotMonthsOut = 0;
		TotWeeksOut = 0;
		TotDaysOut = 0;
		TotHoursOut = 0;
		TotMinutesOut = 0;
	}

	private void ClearCombinedIOVars(Entry ImInFocus)
	{
		// Values for "Combined" dateTime span
		foreach (Entry entry in DictionaryOfCombinedEntries.Keys)
		{
			if (entry != ImInFocus)
			{
				DictionaryOfCombinedEntries[entry] = 0;
			}
		}
		// Combined
		CombndYearsOut = 0;
		CombndMonthsOut = 0;
		CombndWeeksOut = 0;
		CombndDaysOut = 0;
		CombndHoursOut = 0;
		CombndMinutesOut = 0;
	}

	private void ClearCombinedYMWDHM(Entry ImInFocus)
	{
		foreach (Entry CurEntry in DictionaryOfCombinedEntries.Keys)
		{
			if (CurEntry != ImInFocus)
			{
				CurEntry.Text = "";
			}
		}
		ClearCombinedIOVars(ImInFocus);
	}

	private void ClearTotYMWDHM(Entry ImInFocus)
	{
		foreach (Entry CurEntry in DictionaryOfTotalEntries.Keys)
		{
			if (CurEntry != ImInFocus)
			{
				CurEntry.Text = "";
			}
		}
		ClearTotIOVars(ImInFocus);
	}

	private void ClearYMWDHM(Entry ImInFocus)
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

		if (DeviceInfo.Platform == DevicePlatform.iOS)
		{
			CombndYears.WidthRequest = 105;
			CombndMonths.WidthRequest = 105;
			CombndWeeks.WidthRequest = 105;
			CombndDays.WidthRequest = 105;
			CombndHours.WidthRequest = 105;
			CombndMinutes.WidthRequest = 105;

			TotYears.WidthRequest = 105;
			TotMonths.WidthRequest = 105;
			TotWeeks.WidthRequest = 105;
			TotDays.WidthRequest = 105;
			TotHours.WidthRequest = 105;
			TotMinutes.WidthRequest = 105;

		}
		else if (DeviceInfo.Platform == DevicePlatform.Android)
		{
			CombndYears.WidthRequest = 88;
			CombndMonths.WidthRequest = 88;
			CombndWeeks.WidthRequest = 88;
			CombndDays.WidthRequest = 88;
			CombndHours.WidthRequest = 88;
			CombndMinutes.WidthRequest = 88;

			TotYears.WidthRequest = 88;
			TotMonths.WidthRequest = 88;
			TotWeeks.WidthRequest = 88;
			TotDays.WidthRequest = 88;
			TotHours.WidthRequest = 88;
			TotMinutes.WidthRequest = 88;

		}
		else if (DeviceInfo.Platform == DevicePlatform.WinUI)
		{
			CombndYears.WidthRequest = 121;
			CombndMonths.WidthRequest = 121;
			CombndWeeks.WidthRequest = 121;
			CombndDays.WidthRequest = 121;
			CombndHours.WidthRequest = 121;
			CombndMinutes.WidthRequest = 121;

			TotYears.WidthRequest = 121;
			TotMonths.WidthRequest = 121;
			TotWeeks.WidthRequest = 121;
			TotDays.WidthRequest = 121;
			TotHours.WidthRequest = 121;
			TotMinutes.WidthRequest = 121;

		}
		else //Set as UWP
		{
			CombndYears.WidthRequest = 121;
			CombndMonths.WidthRequest = 121;
			CombndWeeks.WidthRequest = 121;
			CombndDays.WidthRequest = 121;
			CombndHours.WidthRequest = 121;
			CombndMinutes.WidthRequest = 121;

			TotYears.WidthRequest = 121;
			TotMonths.WidthRequest = 121;
			TotWeeks.WidthRequest = 121;
			TotDays.WidthRequest = 121;
			TotHours.WidthRequest = 121;
			TotMinutes.WidthRequest = 121;

		}

		ClearAllIOVars();
	}


	private double TotalStackNameScaleLast = 1.0f;
	private double scrollViewNameScaleLast = 1.0f;
	private double ContentPageNameScaleLast = 1.0f;
	private double StartDateTimeStacAndPlusScaleLast = 1.0f;
	private double entriesOuterGridScaleLast = 1.0f;
	private double EndDateTimeAndCalculateAndClearAllButtonsStackNameScaleLast = 1.0f;




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
		else
		{
			if ((EndDateIn == StartDateIn) && (EndTimeIn < StartTimeIn))
			{
				EndTimeIn = StartTimeIn;
				SetEndDateTime();
			}
		}
	}

	private void StartDatePicker_DateSelected(object sender, DateChangedEventArgs e)
	{
		StartDateIn = e.NewDate;

		MacStartDatePicker.Date = StartDateIn;

		StartDayName.Text = StartDateIn.DayOfWeek.ToString().Remove(3);

		CheckSetEndDateTime();
	}

	private void OnMacStartDatePickerDateSelected(object sEnder, DateChangedEventArgs e)
	{
		StartDateIn = e.NewDate;

		StartDatePicker.Date = StartDateIn;

		StartDayName.Text = StartDateIn.DayOfWeek.ToString().Remove(3);

		CheckSetEndDateTime();
	}

	private void OnMacStartTimePickerPropertyChanged(object sEnder, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == "Time")
		{
			//StartTimeIn = MacStartTimePicker.Time;

			StartTimePicker.Time = StartTimeIn;
			//StartTimePicker.Time = new TimeSpan(StartTimeIn.Hours, StartTimeIn.Minutes, 0);

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
			StartTimeIn = StartTimePicker.Time;

			//if
			//(
			//	(MacStartTimePicker.Time.Hours != StartTimeIn.Hours)
			//	||
			//	(MacStartTimePicker.Time.Minutes != StartTimeIn.Minutes)
			//)
			//{
			//	MacStartTimePicker.Time = StartTimeIn;
			//}

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

	private void OnCombinedEntryFocused(object sender, FocusEventArgs e)
	{
		ClearTotYMWDHM(null);
		//((Entry)sender).SelectionLength = ((Entry)sender).Text.Length;
	}

	private void OnCombinedEntryUnfocused(object sender, FocusEventArgs e)
	{
		OnCombinedEntryCompleted(sender, e);
	}

	private async void OnCombinedEntryCompleted(object sEnder, EventArgs args)
	{
		Entry TheEntry = ((Entry)sEnder);
		if (!int.TryParse(TheEntry.Text, out int result) && (TheEntry.Text.Length != 0))
		{
			DictionaryOfCombinedEntries[TheEntry] = 0;
			var TextHolder = TheEntry.Text;
			TheEntry.Text = "";
			await DisplayAlert("Invalid \"Combined Years\" ", TextHolder, "OK");
			TheEntry.Focus();
		}
		else
		{
			DictionaryOfCombinedEntries[TheEntry] = result;
		}
	}

	//TO HERE Combined


	//FROM HERE Total

	private void OnTotEntryFocused(object sender, FocusEventArgs e)
	{
		ClearYMWDHM((Entry)sender);
		//((Entry)sender).SelectionLength = ((Entry)sender).Text.Length;
	}

	private void OnTotEntryUnfocused(object sender, FocusEventArgs e)
	{
		OnTotEntryCompleted(sender, e);
	}

	private async void OnTotEntryCompleted(object sEnder, EventArgs args)
	{
		Entry TheEntry = ((Entry)sEnder);
		if (!int.TryParse(TheEntry.Text, out int result) && (TheEntry.Text.Length != 0))
		{
			DictionaryOfTotalEntries[TheEntry] = 0;
			var TextHolder = TheEntry.Text;
			TheEntry.Text = "";
			await DisplayAlert("Invalid \"Total Years\" ", TextHolder, "OK");
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
		else
		{
			if ((StartDateIn == EndDateIn) && (StartTimeIn > EndTimeIn))
			{
				StartTimeIn = EndTimeIn;
				SetStartDateTime();
			}
		}
	}

	private void EndDatePicker_DateSelected(object sender, DateChangedEventArgs e)
	{
		EndDateIn = e.NewDate;

		MacEndDatePicker.Date = EndDateIn;

		EndDayName.Text = EndDateIn.DayOfWeek.ToString().Remove(3);

		CheckSetStartDateTime();
	}

	private void OnMacEndDatePickerDateSelected(object sEnder, DateChangedEventArgs e)
	{
		EndDateIn = e.NewDate;

		EndDatePicker.Date = EndDateIn;

		EndDayName.Text = EndDateIn.DayOfWeek.ToString().Remove(3);

		CheckSetStartDateTime();
	}

	private void OnMacEndTimePickerPropertyChanged(object sEnder, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == "Time")
		{
			//EndTimeIn = MacEndTimePicker.Time;

			EndTimePicker.Time = EndTimeIn;
			//EndTimePicker.Time = new TimeSpan(EndTimeIn.Hours, EndTimeIn.Minutes, 0);

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
			EndTimeIn = EndTimePicker.Time;

			//if
			//(
			//	(MacEndTimePicker.Time.Hours != EndTimeIn.Hours)
			//	||
			//	(MacEndTimePicker.Time.Minutes != EndTimeIn.Minutes)
			//)
			//{
			//	MacEndTimePicker.Time = EndTimeIn;
			//}

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
	private void ClearAllButtonClicked()
	{
		DoClearAll();
	}



	// CALCULATION from here...

	private async void CalcAndShowTimeSpans()
	{
		CombndYearsOut = EndDateTimeIn.Year - StartDateTimeIn.Year;
		CombndMonthsOut = EndDateTimeIn.Month - StartDateTimeIn.Month;
		//if (EndDateTimeIn.Day < StartDateTimeIn.Day)
		//{
		//	CombndMonthsOut--;
		//}
		if (CombndMonthsOut < 0)
		{
			CombndMonthsOut += 12;
			CombndYearsOut--;
		}
		DateTime dtCalc1 = StartDateTimeIn;
		dtCalc1 = dtCalc1.AddYears(CombndYearsOut);
		dtCalc1 = dtCalc1.AddMonths(CombndMonthsOut);
		TimeSpan ts1 = dtCalc1 - StartDateTimeIn; // Total Days in years + months
		TimeSpan ts2 = EndDateTimeIn - StartDateTimeIn; // Total Days in whole time span
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
			ts1 = dtCalc2 - StartDateTimeIn; // Total Days in years + months
			CombndDaysOut = ts2.Days - ts1.Days;
		}
		CombndHoursOut = ts2.Hours; // Extra days besides Days in years + months
		CombndMinutesOut = ts2.Minutes; // Extra days besides Days in years + months

		CombndWeeksOut = (int)(CombndDaysOut / 7);
		CombndDaysOut %= 7; // Rest after div. w. 7

		TotDaysOut = (Int64)ts2.TotalDays;
		TotWeeksOut = (Int64)(TotDaysOut / 7);
		TotMonthsOut = CombndMonthsOut + 12 * CombndYearsOut;
		TotYearsOut = CombndYearsOut;
		TotHoursOut = (Int64)ts2.TotalHours;
		TotMinutesOut = (Int64)ts2.TotalMinutes;

		// Show Combnd in the text boxes
		CombndDays.Text = CombndDaysOut.ToString();
		CombndWeeks.Text = CombndWeeksOut.ToString();
		CombndMonths.Text = CombndMonthsOut.ToString();
		CombndYears.Text = CombndYearsOut.ToString();
		CombndHours.Text = CombndHoursOut.ToString();
		CombndMinutes.Text = CombndMinutesOut.ToString();

		// Show Tot. in the text boxes
		TotDays.Text = TotDaysOut.ToString();
		if (TotDaysOut > 9999999999)
		{
			await DisplayAlert("Total \"Days\" > 9999999999", TotDays.ToString(), "OK");
		}
		TotWeeks.Text = TotWeeksOut.ToString();
		TotMonths.Text = TotMonthsOut.ToString();
		TotYears.Text = TotYearsOut.ToString();
		TotHours.Text = TotHoursOut.ToString();
		if (TotHoursOut > 9999999999)
		{
			await DisplayAlert("Total \"Hours\" > 9999999999", TotHours.ToString(), "OK");
		}
		TotMinutes.Text = TotMinutesOut.ToString();
		if (TotMinutesOut > 9999999999)
		{
			await DisplayAlert("Total \"Minutes\" > 9999999999", TotMinutes.ToString(), "OK");
		}
	}

	private void OnCalculateButtonClicked(object sEnder, EventArgs e)
	{
		DoCalculate();
	}

	enum EntryNames : int
	{
		years = 0
		,
		months = 1
		,
		weeks = 2
		,
		days = 3
		,
		hours = 4
		,
		minutes = 5
	}

	private async void DoCalculate()
	{
		StartTimeIn = StartTimePicker.Time;
		EndTimeIn = EndTimePicker.Time;

		StartDateIn = StartDatePicker.Date;
		EndDateIn = EndDatePicker.Date;

		// Input values
		EndDateTimeIn = EndDateIn + EndTimeIn;
		StartDateTimeIn = StartDateIn + StartTimeIn;

		// Output values
		StartDateTimeOut = DateTime.MaxValue;
		EndDateTimeOut = DateTime.MaxValue;

		if (DoCalcYMWDHM)
		{
			CalcAndShowTimeSpans();
		}
		else
		{ // !DoCalcYMWDHM
		  // Read all controls
		  // Combined
			foreach (Entry CurEntry in DictionaryOfCombinedEntries.Keys)
			{
				if (!int.TryParse(CurEntry.Text, out int result) && (CurEntry.Text.Length != 0))
				{
					DictionaryOfCombinedEntries[CurEntry] = 0;
					string TextHolder = CurEntry.Text;
					CurEntry.Text = "";
					await DisplayAlert("Invalid \"Combined Value\" ", TextHolder, "OK");
					CurEntry.Focus();
					return;
				}
				else
				{
					DictionaryOfCombinedEntries[CurEntry] = result;
				}
			}
			// Total
			foreach (Entry CurEntry in DictionaryOfTotalEntries.Keys)
			{
				if (!int.TryParse(CurEntry.Text, out int result) && (CurEntry.Text.Length != 0))
				{
					DictionaryOfTotalEntries[CurEntry] = 0;
					string TextHolder = CurEntry.Text;
					CurEntry.Text = "";
					await DisplayAlert("Invalid \"Total Value\" ", TextHolder, "OK");
					CurEntry.Focus();
					return;
				}
				else
				{
					DictionaryOfTotalEntries[CurEntry] = result;
				}
			}
		} // if (DoCalcYMWDHM) ..else


		bool TotalsAllZero = true;
		foreach (int TheValue in DictionaryOfTotalEntries.Values)
		{
			TotalsAllZero &= TheValue == 0;
		}

		bool CombinedsAllZero = true;
		foreach (int TheValue in DictionaryOfCombinedEntries.Values)
		{
			CombinedsAllZero &= TheValue == 0;
		}

		if (DoCalcEndTime)
		{ // DoCalcEndTime = true
			if (!TotalsAllZero || !CombinedsAllZero)
			{
				if (TotalsAllZero || CombinedsAllZero)
				{
					EndDateTimeOut = DateTime.MaxValue; // <=> no EndDateTimeOut found

					if (!TotalsAllZero)
					{
						for (int i = 0; i < DictionaryOfTotalEntries.Count; i++)
						{
							if (DictionaryOfTotalEntries.ElementAt(i).Value != 0)
							{
								bool RestIsZero = true;
								for (int j = i + 1; j < DictionaryOfTotalEntries.Count; j++)
								{
									RestIsZero &= DictionaryOfTotalEntries.ElementAt(j).Value == 0;
								}

								if (RestIsZero)
								{
									try
									{
										switch (i)
										{
											case (int)EntryNames.years:
												{
													EndDateTimeOut =
														StartDateTimeIn.AddYears(DictionaryOfTotalEntries.ElementAt(i).Value);
													break;
												}
											case (int)EntryNames.months:
												{
													EndDateTimeOut =
														StartDateTimeIn.AddMonths(DictionaryOfTotalEntries.ElementAt(i).Value);
													break;
												}
											case (int)EntryNames.weeks:
												{
													EndDateTimeOut =
														StartDateTimeIn.AddDays((DictionaryOfTotalEntries.ElementAt(i).Value) * 7);
													break;
												}
											case (int)EntryNames.days:
												{
													EndDateTimeOut =
														StartDateTimeIn.AddDays(DictionaryOfTotalEntries.ElementAt(i).Value);
													break;
												}
											case (int)EntryNames.hours:
												{
													EndDateTimeOut =
														StartDateTimeIn.AddHours(DictionaryOfTotalEntries.ElementAt(i).Value);
													break;
												}
											case (int)EntryNames.minutes:
												{
													EndDateTimeOut =
														StartDateTimeIn.AddMinutes(DictionaryOfTotalEntries.ElementAt(i).Value);
													break;
												}
											default:
												break;
										}
									}
									catch (ArgumentOutOfRangeException outOfRange)
									{
										await DisplayAlert
										   (
											   "Argument Out Of Range"
											   , outOfRange.Message.Remove(outOfRange.Message.IndexOf(" name:")) + ": \"Total Years\" added = " + DictionaryOfTotalEntries.ElementAt(i).Value.ToString()
											   + ".\r\nDate+Time Max. Value is " + DateTime.MaxValue.ToString("u").Remove(16)
											   , "OK"
										   );
										DictionaryOfTotalEntries[DictionaryOfTotalEntries.ElementAt(i).Key] = 0;
										DictionaryOfTotalEntries.ElementAt(i).Key.Text = "";
										DictionaryOfTotalEntries.ElementAt(i).Key.Focus();
										return;
									}
								} // if (RestIsZero)
								else
								{
									await DisplayAlert
									   (
										   "Type error"
										   , "Only one \"Total\" TheValue allowed"
										   , "OK"
									   );
								}
							}
						}
					} // if (!TotalsAllZero)
					else
					{ // Must be Combined time span

						EndDateTimeOut = StartDateTimeIn;

						int i = 0;
						foreach (KeyValuePair<Entry, int> TheKeyValuePair in DictionaryOfCombinedEntries)
						{
							if (TheKeyValuePair.Value != 0)
							{
								try
								{
									switch (i)
									{
										case (int)EntryNames.years:
											{
												EndDateTimeOut =
													EndDateTimeOut.AddYears(TheKeyValuePair.Value);
												break;
											}
										case (int)EntryNames.months:
											{
												EndDateTimeOut =
													EndDateTimeOut.AddMonths(TheKeyValuePair.Value);
												break;
											}
										case (int)EntryNames.weeks:
											{
												EndDateTimeOut =
													EndDateTimeOut.AddDays(TheKeyValuePair.Value * 7);
												break;
											}
										case (int)EntryNames.days:
											{
												EndDateTimeOut =
													EndDateTimeOut.AddDays(TheKeyValuePair.Value);
												break;
											}
										case (int)EntryNames.hours:
											{
												EndDateTimeOut =
													EndDateTimeOut.AddHours(TheKeyValuePair.Value);
												break;
											}
										case (int)EntryNames.minutes:
											{
												EndDateTimeOut =
													EndDateTimeOut.AddMinutes(TheKeyValuePair.Value);
												break;
											}
										default:
											break;
									}
								}
								catch (ArgumentOutOfRangeException outOfRange)
								{
									await DisplayAlert
									   (
										   "Argument Out Of Range"
										   , outOfRange.Message.Remove(outOfRange.Message.IndexOf(" name:")) + ": \"Combined Years\" added = " + TheKeyValuePair.Key.ToString()
										   + ".\r\nDate+Time Max. Value is " + DateTime.MaxValue.ToString("u").Remove(16)
										   , "OK"
									   );
									DictionaryOfCombinedEntries[TheKeyValuePair.Key] = 0;
									TheKeyValuePair.Key.Text = "";
									TheKeyValuePair.Key.Focus();
									return;
								}
							}

							i++;
						}

					}  // if (!TotalsAllZero) ... else ...

					if (EndDateTimeOut != DateTime.MaxValue)
					{
						// Save tmp SartDateTime and EndDateTime
						var tmpDoCalcEndTime = DoCalcEndTime;

						// Clear and reseteverything
						DoClearAll();

						// Show Start- and End Date Time
						DoCalcEndTime = tmpDoCalcEndTime;

						EndDateTimeIn = EndDateTimeOut;
						EndDateIn = EndDateTimeOut.Date;
						EndTimeIn = EndDateTimeOut.TimeOfDay;

						SetEndDateTime();

						// Show Time Spans.
						CalcAndShowTimeSpans();
					}

				} // if ( !(!TotalsAllZero && !CombinedsAllZero) )
				else
				{
					await DisplayAlert
					   (
						   "Type error"
						   , "Not both \"Total\" and \"Combined\" time spans can be used"
						   , "OK"
					   );
				} // if ( !(!TotalsAllZero && !CombinedsAllZero) ) ... else ...
			} // if ( !(TotalsAllZero && CombinedsAllZero) )
			else
			{
				// Output values
				EndDateTimeOut = StartDateTimeIn;
				// Save tmp SartDateTime and EndDateTime
				var tmpDoCalcEndTime = DoCalcEndTime;

				// Clear and reseteverything
				DoClearAll();

				// Show Start- and End Date Time
				DoCalcEndTime = tmpDoCalcEndTime;

				EndDateTimeIn = EndDateTimeOut;
				EndDateIn = EndDateTimeOut.Date;
				EndTimeIn = EndDateTimeOut.TimeOfDay;

				SetEndDateTime();

				// Show Time Spans.
				CalcAndShowTimeSpans();

			} //  // if ( !(TotalsAllZero && CombinedsAllZero) ) ... else ...
		} // if (!DoCalcEndTime) ... else ...

		if (DoCalcStartTime)
		{ // DoCalcStartTime = true
			if (!DoCalcEndTime)
			{
				if (!(TotalsAllZero && CombinedsAllZero))
				{
					if (!(!TotalsAllZero && !CombinedsAllZero))
					{
						StartDateTimeOut = DateTime.MaxValue; // <=> no StartDateTimeOut found

						if (!TotalsAllZero)
						{
							for (int i = 0; i < DictionaryOfTotalEntries.Count; i++)
							{
								if (DictionaryOfTotalEntries.ElementAt(i).Value != 0)
								{
									bool RestIsZero = true;
									for (int j = i + 1; j < DictionaryOfTotalEntries.Count; j++)
									{
										RestIsZero &= DictionaryOfTotalEntries.ElementAt(j).Value == 0;
									}

									if (RestIsZero)
									{
										try
										{
											switch (i)
											{
												case (int)EntryNames.years:
													{
														StartDateTimeOut =
															EndDateTimeIn.AddYears(-(DictionaryOfTotalEntries.ElementAt(i).Value));
														break;
													}
												case (int)EntryNames.months:
													{
														StartDateTimeOut =
															EndDateTimeIn.AddMonths(-(DictionaryOfTotalEntries.ElementAt(i).Value));
														break;
													}
												case (int)EntryNames.weeks:
													{
														StartDateTimeOut =
															EndDateTimeIn.AddDays(-((DictionaryOfTotalEntries.ElementAt(i).Value) * 7));
														break;
													}
												case (int)EntryNames.days:
													{
														StartDateTimeOut =
															EndDateTimeIn.AddDays(-(DictionaryOfTotalEntries.ElementAt(i).Value));
														break;
													}
												case (int)EntryNames.hours:
													{
														StartDateTimeOut =
															EndDateTimeIn.AddHours(-(DictionaryOfTotalEntries.ElementAt(i).Value));
														break;
													}
												case (int)EntryNames.minutes:
													{
														StartDateTimeOut =
															EndDateTimeIn.AddMinutes(-(DictionaryOfTotalEntries.ElementAt(i).Value));
														break;
													}
												default:
													break;
											}
										}
										catch (ArgumentOutOfRangeException outOfRange)
										{
											await DisplayAlert
											   (
												   "Argument Out Of Range"
												   , outOfRange.Message.Remove(outOfRange.Message.IndexOf(" name:")) + ": \"Total Years\" added = " + DictionaryOfTotalEntries.ElementAt(i).Value.ToString()
												   + ".\r\nDate+Time Max. Value is " + DateTime.MaxValue.ToString("u").Remove(16)
												   , "OK"
											   );
											DictionaryOfTotalEntries[DictionaryOfTotalEntries.ElementAt(i).Key] = 0;
											DictionaryOfTotalEntries.ElementAt(i).Key.Text = "";
											DictionaryOfTotalEntries.ElementAt(i).Key.Focus();
											return;
										}
									} // if (RestIsZero)
									else
									{
										await DisplayAlert
										   (
											   "Type error"
											   , "Only one \"Total\" TheValue allowed"
											   , "OK"
										   );
									}
								}
							}
						} // if (!TotalsAllZero)
						else
						{ // Must be Combnd time span

							StartDateTimeOut = EndDateTimeIn;

							int i = 0;
							foreach (KeyValuePair<Entry, int> TheKeyValuePair in DictionaryOfCombinedEntries)
							{
								if (TheKeyValuePair.Value != 0)
								{
									try
									{
										switch (i)
										{
											case (int)EntryNames.years:
												{
													StartDateTimeOut =
														StartDateTimeOut.AddYears(-(TheKeyValuePair.Value));
													break;
												}
											case (int)EntryNames.months:
												{
													StartDateTimeOut =
														StartDateTimeOut.AddMonths(-(TheKeyValuePair.Value));
													break;
												}
											case (int)EntryNames.weeks:
												{
													StartDateTimeOut =
														StartDateTimeOut.AddDays(-((TheKeyValuePair.Value) * 7));
													break;
												}
											case (int)EntryNames.days:
												{
													StartDateTimeOut =
														StartDateTimeOut.AddDays(-(TheKeyValuePair.Value));
													break;
												}
											case (int)EntryNames.hours:
												{
													StartDateTimeOut =
														StartDateTimeOut.AddHours(-(TheKeyValuePair.Value));
													break;
												}
											case (int)EntryNames.minutes:
												{
													StartDateTimeOut =
														StartDateTimeOut.AddMinutes(-(TheKeyValuePair.Value));
													break;
												}
											default:
												break;
										}
									}
									catch (ArgumentOutOfRangeException outOfRange)
									{
										await DisplayAlert
										   (
											   "Argument Out Of Range"
											   , outOfRange.Message.Remove(outOfRange.Message.IndexOf(" name:")) + ": \"Combined Years\" added = " + TheKeyValuePair.Key.ToString()
											   + ".\r\nDate+Time Max. Value is " + DateTime.MaxValue.ToString("u").Remove(16)
											   , "OK"
										   );
										DictionaryOfCombinedEntries[TheKeyValuePair.Key] = 0;
										TheKeyValuePair.Key.Text = "";
										TheKeyValuePair.Key.Focus();
										return;
									}
								}

								i++;
							}

						}  // if (!TotalsAllZero) ... else ...

						if (StartDateTimeOut != DateTime.MaxValue)
						{
							// Save tmp SartDateTime and EndDateTime
							var tmpDoCalcStartTime = DoCalcStartTime;

							// Clear and reseteverything
							DoClearAll();

							//// Show Start- and End Date Time
							DoCalcStartTime = tmpDoCalcStartTime;
							StartDateTimeIn = StartDateTimeOut;

							StartDateIn = StartDateTimeOut.Date;
							StartTimeIn = StartDateTimeOut.TimeOfDay;

							SetStartDateTime();

							// Show Time Spans.
							CalcAndShowTimeSpans();
						}

					} // if ( !(!TotalsAllZero && !CombinedsAllZero) )
					else
					{
						await DisplayAlert
						   (
							   "Type error"
							   , "Not both \"Total\" and \"Combined\" time spans can be used"
							   , "OK"
						   );
					} // if ( !(!TotalsAllZero && !CombinedsAllZero) ) ... else ...
				} // if ( !(TotalsAllZero && CombinedsAllZero) )
				else
				{

					// Output values
					StartDateTimeOut = EndDateTimeIn;
					// Save tmp SartDateTime and EndDateTime
					var tmpDoCalcStartTime = DoCalcStartTime;

					// Clear and reseteverything
					DoClearAll();

					//// Show Start- and End Date Time
					DoCalcStartTime = tmpDoCalcStartTime;
					StartDateTimeIn = StartDateTimeOut;

					StartDateIn = StartDateTimeOut.Date;
					StartTimeIn = StartDateTimeOut.TimeOfDay;

					SetStartDateTime();

					// Show Time Spans.
					CalcAndShowTimeSpans();

				} //  // if ( !(TotalsAllZero && CombinedsAllZero) ) ... else ...
			} // if (!DoCalcEndTime)
			else
			{ // DoCalcEndTime = true
				await DisplayAlert
				   (
					   "Error"
					   , "Can't calculate both \"Start Date + Time\" and \"End Date + Time\""
					   , "OK"
				   );
			} // if (!DoCalcEndTime) ... else ...
		} // if (!DoCalcStartTime) ... else...

	} // private async void OnCalculateButtonClicked(object sEnder, EventArgs e)

	// CALCULATION Ends here...

	[RelayCommand]
	private async Task HelpButtonClicked()
	{
		await Shell.Current.GoToAsync
		(
			nameof(AboutHelp)
			, true
		);
	}

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

	// Calendar
	private string CalendarItem = "";
	private bool CorrectForIcsTimeZone = false;

	private readonly string filetypeToReadFrom = "ics";
	private readonly string filetypeToSaveTo = "ics";

	private async void On_OpenIcsMessageReceived(object recipient, OpenIcsMessageArgs message)
	{
		CorrectForIcsTimeZone = message.CorrectForTimeZone;

		SelectFilesResult selectedFiles = await OLD_FileHandler.SelectFiles(filetypeToReadFrom);

		On_FileToReadFromSelectedAsync(selectedFiles);

	}

	private async void On_FileToReadFromSelectedAsync(SelectFilesResult arg2)
	{
		if (arg2.DidPick)
		{

			List<string> TheIcsTxt = new List<string>();
			try
			{
				// Create an instance of StreamReader to read from a file.
				// The using statement also closes the StreamReader.
				using StreamReader sr = new StreamReader(await arg2.pickResult.OpenReadAsync());
				string line;
				// Read and display lines from the file until the end of
				// the file is reached.
				while ((line = sr.ReadLine()) != null)
				{
					TheIcsTxt.Add(line);
				}
			}
			catch (Exception e)
			{
				// Let the user know what went wrong.
				await DisplayAlert
						   (
							   "The file could not be read:"
							   , e.Message
							   , "OK"
						   );
			}

			try
			{
				// Time Zone
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
				if (TheSign == '-')
				{
					TheTZOFFSETTO = TimeSpan.Zero - TheTZOFFSETTO;
				}
				var BaseUtcOff = TimeZoneInfo.Local.BaseUtcOffset;

				// Start Time
				TimeIDX = TheIcsTxt.FindIndex(s => s.Contains(@"DTSTART;TZID="));
				StartOfTimeStringIDX = TheIcsTxt[TimeIDX].LastIndexOf(':') + 1;
				LgthOfTimestring = TheIcsTxt[TimeIDX].Length - StartOfTimeStringIDX;
				TimeString = TheIcsTxt[TimeIDX].Substring(StartOfTimeStringIDX, LgthOfTimestring);
				StartDateTimeOut = DateTime.ParseExact(TimeString, @"yyyyMMddTHHmm00", null);

				if (CorrectForIcsTimeZone)
				{
					StartDateTimeOut -= TheTZOFFSETTO; // Calender start time in utc time
					StartDateTimeOut += BaseUtcOff; // In local time zone time
				}

				StartDateTimeIn = StartDateTimeOut;
				StartDateIn = StartDateTimeOut.Date;
				StartTimeIn = StartDateTimeOut.TimeOfDay;

				SetStartDateTime();

				// End Date Time
				TimeIDX = TheIcsTxt.FindIndex(s => s.Contains(@"DTEND;TZID="));
				StartOfTimeStringIDX = TheIcsTxt[TimeIDX].LastIndexOf(':') + 1;
				LgthOfTimestring = TheIcsTxt[TimeIDX].Length - StartOfTimeStringIDX;
				TimeString = TheIcsTxt[TimeIDX].Substring(StartOfTimeStringIDX, LgthOfTimestring);
				EndDateTimeOut = DateTime.ParseExact(TimeString, @"yyyyMMddTHHmm00", null);

				if (CorrectForIcsTimeZone)
				{
					EndDateTimeOut -= TheTZOFFSETTO; // Calender End time in utc time
					EndDateTimeOut += BaseUtcOff; // In local time zone time
				}

				EndDateTimeIn = EndDateTimeOut;
				EndDateIn = EndDateTimeOut.Date;
				EndTimeIn = EndDateTimeOut.TimeOfDay;

				SetEndDateTime();

				// Show Time Spans.
				CalcAndShowTimeSpans();

			}
			catch (Exception e)
			{
				await DisplayAlert
						   (
							   "Bad .ics file"
							   , e.Message
							   , "OK"
						   );
			}

			await Shell.Current.GoToAsync
			(
				"..\\.."
				, true
			);

		}
	}

	string SuggestedNameOfFileToSaveTo = "";
	private async void On_SaveToIcsMessageReceived(object recipient, SaveToIcsMessageArgs message)
	{
		DateTime DateStart = StartDateIn + StartTimeIn;
		DateTime DateEnd = EndDateIn + EndTimeIn;
		string Summary = message.EventName_Summary;
		string Location = message.Location;
		string Description = message.TheDescription;
		//string FileName = "CalendarItem";

		//create a new stringbuilder instance
		StringBuilder sb = new StringBuilder();

		//start the calendar item
		sb.AppendLine("BEGIN:VCALENDAR");
		sb.AppendLine("VERSION:2.0");
		sb.AppendLine("PRODID:eksit.dk");
		//sb.AppendLine("CALSCALE:GREGORIAN");
		sb.AppendLine("METHOD:PUBLISH");

#if true // USE_LOCAL_TIME
		var TimeZoneName = TimeZoneInfo.Local.StandardName;
		var systemTimeZoneName = TimeZoneInfo.GetSystemTimeZones();
		var IsDaylightsavingtimeOn = TimeZoneInfo.Local.IsDaylightSavingTime(DateTime.Now);
		var UtcOffset = TimeZoneInfo.Local.GetUtcOffset(DateTime.Now);
		var UtcOffsetStr = UtcOffset.ToString("hhmm");
		if (UtcOffset.Hours >= 0)
		{
			UtcOffsetStr = "+" + UtcOffsetStr;
		}
		else
		{
			UtcOffsetStr = "-" + UtcOffsetStr;
		}

		var BaseUtcOff = TimeZoneInfo.Local.BaseUtcOffset;
		var BaseUtcOffStr = BaseUtcOff.ToString("hhmm");
		if (BaseUtcOff.Hours >= 0)
		{
			BaseUtcOffStr = "+" + BaseUtcOffStr;
		}
		else
		{
			BaseUtcOffStr = "-" + BaseUtcOffStr;
		}
#else // USE_LOCAL_TIME (USE "Central Standard Time")
		TimeZoneInfo cst = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
		var TimeZoneName = cst.StandardName;
		var UtcOffset = cst.GetUtcOffset(DateTime.Now);
		var UtcOffsetStr = UtcOffset.ToString("hhmm");
		if (UtcOffset.Hours >= 0)
		{
			UtcOffsetStr = "+" + UtcOffsetStr;
		}
		else
		{
			UtcOffsetStr = "-" + UtcOffsetStr;
		}

		var BaseUtcOff = cst.BaseUtcOffset;
		var BaseUtcOffStr = BaseUtcOff.ToString("hhmm");
		if (BaseUtcOff.Hours >= 0)
		{
			BaseUtcOffStr = "+" + BaseUtcOffStr;
		}
		else
		{
			BaseUtcOffStr = "-" + BaseUtcOffStr;
		}
#endif // USE_LOCAL_TIME

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

		//add the event
		sb.AppendLine("BEGIN:VEVENT");

		//with time zone specified
		sb.AppendLine("DTSTART;TZID=" + "\"" + TimeZoneName + "\":" + DateStart.ToString("yyyyMMddTHHmm00"));
		sb.AppendLine("DTEND;TZID=" + "\"" + TimeZoneName + "\":" + DateEnd.ToString("yyyyMMddTHHmm00"));
		//or without
		//sb.AppendLine("DTSTART:" + DateStart.ToString("yyyyMMddTHHmm00"));
		//sb.AppendLine("DTEND:" + DateEnd.ToString("yyyyMMddTHHmm00"));

		sb.AppendLine("SUMMARY:" + Summary + "");
		sb.AppendLine("LOCATION:" + Location + "");
		sb.AppendLine("DESCRIPTION:" + Description + "");
		sb.AppendLine("PRIORITY:5");

		sb.AppendLine("END:VEVENT");

		//end calendar item
		sb.AppendLine("END:VCALENDAR");

		CalendarItem = sb.ToString().Replace("\r", "");
		//send the calendar item to the browser
		//Response.ClearHeaders();
		//Response.Clear();
		//Response.Buffer = true;
		//Response.ContentType = "text/calendar";
		//Response.AddHeader("content-length", CalendarItem.Length.ToString());
		//Response.AddHeader("content-disposition", "attachment; filename=\"" + FileName + ".ics\"");
		//Response.Write(CalendarItem);
		//Response.Flush();
		//HttpContext.Current.ApplicationInstance.CompleteRequest();


		string[] filetypesToSaveTo = new string[] { "ics" };
		SuggestedNameOfFileToSaveTo = Summary;

		using MemoryStream stream = new MemoryStream(Encoding.Default.GetBytes(CalendarItem));

		FileSaverResult fileSaveResult = await OLD_FileHandler.SaveToTextFile(stream, "Calendar.ics");

		// Close file
		stream.Dispose();

		if (fileSaveResult.IsSuccessful)
		{
			await Shell.Current.GoToAsync
			(
				"..\\.."
				, true
			);
		}
	}

	[RelayCommand]
	private async Task FileButton_Clicked()
	{
		await Shell.Current.GoToAsync
		(
			nameof(FileICS)
			, true
		);
	}

	private async void On_FileToSaveToSelected(SelectFilesResult arg2)
	{
		if (arg2.DidPick)
		{
			using MemoryStream stream = new MemoryStream(Encoding.Default.GetBytes(CalendarItem));

			FileSaverResult fileSaveResult = await OLD_FileHandler.SaveToTextFile(stream, arg2.pickResult.FullPath);

			// Close file
			stream.Dispose();
		}
	}

}
