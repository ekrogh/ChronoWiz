using System;
using System.Collections.Generic;
using System.Globalization;

namespace ChronoWiz.Shared.Ics;

public static class IcsParser
{
	public static IcsParseResult ParseStartEndFromText(IEnumerable<string> lines, bool correctForIcsTimeZone)
	{
		var list = lines is List<string> l ? l : new List<string>(lines);

		var idxBeginStandard = list.FindIndex(s => s.Contains("BEGIN:STANDARD", StringComparison.Ordinal));
		var idxEndStandard = list.FindIndex(s => s.Contains("END:STANDARD", StringComparison.Ordinal));
		if (idxBeginStandard < 0 || idxEndStandard < 0 || idxEndStandard <= idxBeginStandard)
			throw new FormatException("BEGIN:STANDARD/END:STANDARD section not found.");

		var lenStandard = idxEndStandard - idxBeginStandard;
		var timeIdx = list.FindIndex(idxBeginStandard, lenStandard, s => s.Contains("TZOFFSETTO:", StringComparison.Ordinal));
		if (timeIdx < 0)
			throw new FormatException("TZOFFSETTO not found.");

		var line = list[timeIdx];
		var signIdx = line.IndexOfAny([ '+', '-' ], line.LastIndexOf(':'));
		if (signIdx < 0)
			throw new FormatException("TZOFFSETTO sign not found.");

		var sign = line[signIdx];
		var timeString = line[(signIdx + 1)..].Trim();
		var tzOffsetTo = TimeSpan.ParseExact(timeString, "hhmm", CultureInfo.InvariantCulture);
		if (sign == '-') tzOffsetTo = TimeSpan.Zero - tzOffsetTo;

		var baseUtcOff = TimeZoneInfo.Local.BaseUtcOffset;

		DateTime Parse(string token)
		{
			var idx = list.FindIndex(s => s.Contains(token, StringComparison.Ordinal));
			if (idx < 0)
				throw new FormatException($"{token} not found.");

			var startIdx = list[idx].LastIndexOf(':') + 1;
			if (startIdx <= 0 || startIdx >= list[idx].Length)
				throw new FormatException($"{token} value not found.");

			var value = list[idx][startIdx..];
			var dt = DateTime.ParseExact(value, "yyyyMMdd'T'HHmm00", CultureInfo.InvariantCulture);
			if (correctForIcsTimeZone)
			{
				dt -= tzOffsetTo;
				dt += baseUtcOff;
			}

			return dt;
		}

		return new IcsParseResult
		{
			Start = Parse("DTSTART;TZID="),
			End = Parse("DTEND;TZID=")
		};
	}
}
