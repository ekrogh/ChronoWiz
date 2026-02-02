using System;

namespace ChronoWiz.Avalonia.Linux.Navigation;

public sealed class NavigationService : INavigationService
{
	private readonly Action<string> _navigate;
	private readonly Func<bool>? _canGoBack;
	private readonly Action? _goBack;

	public NavigationService(Action<string> navigate, Func<bool>? canGoBack = null, Action? goBack = null)
	{
		_navigate = navigate;
		_canGoBack = canGoBack;
		_goBack = goBack;
	}

	public void NavigateTo(string route) => _navigate(route);

	public bool CanGoBack => _canGoBack?.Invoke() ?? false;

	public void GoBack() => _goBack?.Invoke();
}
