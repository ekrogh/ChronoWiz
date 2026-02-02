using System;

namespace ChronoWiz.Avalonia.Linux.Navigation;

public sealed class NavigationService : INavigationService
{
	private readonly Action<string> _navigate;

	public NavigationService(Action<string> navigate)
	{
		_navigate = navigate;
	}

	public void NavigateTo(string route) => _navigate(route);
}
