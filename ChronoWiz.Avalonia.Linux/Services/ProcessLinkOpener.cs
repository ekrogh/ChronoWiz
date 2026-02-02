using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ChronoWiz.Avalonia.Linux.Services;

public sealed class ProcessLinkOpener : ILinkOpener
{
	public Task OpenAsync(Uri uri)
	{
		try
		{
			Process.Start(new ProcessStartInfo
			{
				FileName = uri.ToString(),
				UseShellExecute = true
			});
		}
		catch
		{
			// best-effort, ignore
		}

		return Task.CompletedTask;
	}
}
