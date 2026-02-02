using System;
using System.Threading.Tasks;

namespace ChronoWiz.Avalonia.Linux.Services;

public interface ILinkOpener
{
	Task OpenAsync(Uri uri);
}
