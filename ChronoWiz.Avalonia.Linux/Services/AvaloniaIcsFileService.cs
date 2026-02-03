using Avalonia.Controls;
using Avalonia.Platform.Storage;
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
		var storage = _owner.StorageProvider;
		if (storage is null)
			return null;

		var results = await storage.OpenFilePickerAsync(new FilePickerOpenOptions
		{
			AllowMultiple = false,
			FileTypeFilter = new List<FilePickerFileType>
			{
				new FilePickerFileType("iCalendar") { Patterns = new[] { "*.ics" } },
				FilePickerFileTypes.All
			}
		});

		var file = results.FirstOrDefault();
		if (file is null)
			return null;

		var lines = new List<string>();
		await using (var stream = await file.OpenReadAsync())
		using (var sr = new StreamReader(stream))
		{
			while (await sr.ReadLineAsync() is { } line)
				lines.Add(line);
		}

		return IcsParser.ParseStartEndFromText(lines, correctForTimeZone);
	}

	public async Task<bool> PickAndSaveIcsAsync(string icsContent)
	{
		var storage = _owner.StorageProvider;
		if (storage is null)
			return false;

		var file = await storage.SaveFilePickerAsync(new FilePickerSaveOptions
		{
			DefaultExtension = "ics",
			FileTypeChoices = new List<FilePickerFileType>
			{
				new FilePickerFileType("iCalendar") { Patterns = new[] { "*.ics" } },
				FilePickerFileTypes.All
			}
		});

		if (file is null)
			return false;

		await using (var stream = await file.OpenWriteAsync())
		using (var sw = new StreamWriter(stream))		
		{
			await sw.WriteAsync(icsContent);
		}

		return true;
	}
}
