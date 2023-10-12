namespace TimeCalculator.MessageThings;

public class SaveToIcsMessageArgs : EventArgs
{
	public string TheDescription;
	public string EventName_Summary;
	public string Location;
}
public class OpenIcsMessageArgs : EventArgs
{
	public bool CorrectForTimeZone = false;
}
