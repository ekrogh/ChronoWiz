using Avalonia.Controls;
using ChronoWiz.Shared.Ics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ChronoWiz.Avalonia.Linux.Services;

public sealed class AvaloniaIcsFileService : IIcsFileService
{
	private readonly Window _owner;

	public AvaloniaIcsFileService(Window owner)
	{
		_owner = owner;
	}

	public async Task<IcsParseResult?> PickAndReadIcsAsync(bool correctForTimeZone)
	{
		var picker = new OpenFileDialog
		{
			AllowMultiple = false,
			Filters =
			{
				new FileDialogFilter { Name = "iCalendar", Extensions = { "ics" } },
				new FileDialogFilter { Name = "All", Extensions = { "*" } }
			}
		};

		var results = await picker.ShowAsync(_owner);
		var path = results?.FirstOrDefault();
		if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
			return null;

		List<string> lines;
		using (var sr = new StreamReader(path))
		{
			lines = new List<string>();
			while (await sr.ReadLineAsync() is { } line)
				lines.Add(line);
		}

		return IcsParser.ParseStartEndFromText(lines, correctForTimeZone);
	}

	public async Task<bool> PickAndSaveIcsAsync(string icsContent)
	{
		var picker = new SaveFileDialog
		{
			DefaultExtension = "ics",
			Filters =
			{
				new FileDialogFilter { Name = "iCalendar", Extensions = { "ics" } },
				new FileDialogFilter { Name = "All", Extensions = { "*" } }
			}
		};

		var path = await picker.ShowAsync(_owner);
		if (string.IsNullOrWhiteSpace(path))
			return false;

		await File.WriteAllTextAsync(path, icsContent);
		return true;
	}
}
