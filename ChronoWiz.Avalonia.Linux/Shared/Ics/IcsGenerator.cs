using System;
using System.Text;

namespace ChronoWiz.Shared.Ics;

public static class IcsGenerator
{
	public static string GenerateCalendar(DateTime start, DateTime end, string summary, string description, string location)
	{
		var timeZoneName = TimeZoneInfo.Local.StandardName;
		var utcOffset = TimeZoneInfo.Local.GetUtcOffset(DateTime.Now);
		var utcOffsetStr = (utcOffset.Hours >= 0 ? "+" : "-") + utcOffset.ToString("hhmm");
		var baseUtcOff = TimeZoneInfo.Local.BaseUtcOffset;
		var baseUtcOffStr = (baseUtcOff.Hours >= 0 ? "+" : "-") + baseUtcOff.ToString("hhmm");

		var sb = new StringBuilder();
		sb.AppendLine("BEGIN:VCALENDAR");
		sb.AppendLine("VERSION:2.0");
		sb.AppendLine("PRODID:eksit.dk");
		sb.AppendLine("METHOD:PUBLISH");
		sb.AppendLine("BEGIN:VTIMEZONE");
		sb.AppendLine("TZID:" + timeZoneName);
		sb.AppendLine("BEGIN:STANDARD");
		sb.AppendLine("TZOFFSETFROM:" + utcOffsetStr);
		sb.AppendLine("TZOFFSETTO:" + baseUtcOffStr);
		sb.AppendLine("END:STANDARD");
		sb.AppendLine("BEGIN:DAYLIGHT");
		sb.AppendLine("TZOFFSETFROM:" + baseUtcOffStr);
		sb.AppendLine("TZOFFSETTO:" + utcOffsetStr);
		sb.AppendLine("END:DAYLIGHT");
		sb.AppendLine("END:VTIMEZONE");
		sb.AppendLine("BEGIN:VEVENT");
		sb.AppendLine("DTSTART;TZID=\"" + timeZoneName + "\":" + start.ToString("yyyyMMddTHHmm00"));
		sb.AppendLine("DTEND;TZID=\"" + timeZoneName + "\":" + end.ToString("yyyyMMddTHHmm00"));
		sb.AppendLine("SUMMARY:" + summary);
		sb.AppendLine("LOCATION:" + location);
		sb.AppendLine("DESCRIPTION:" + description);
		sb.AppendLine("PRIORITY:5");
		sb.AppendLine("END:VEVENT");
		sb.AppendLine("END:VCALENDAR");

		return sb.ToString().Replace("\r", "");
	}
}
