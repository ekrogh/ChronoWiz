using ChronoWiz.Shared.Ics;
using System.Threading.Tasks;

namespace ChronoWiz.Avalonia.Linux.Services;

public interface IIcsFileService
{
	Task<IcsParseResult?> PickAndReadIcsAsync(bool correctForTimeZone);
	Task<bool> PickAndSaveIcsAsync(string icsContent);
}
