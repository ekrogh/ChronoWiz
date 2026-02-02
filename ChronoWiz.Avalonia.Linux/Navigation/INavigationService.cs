namespace ChronoWiz.Avalonia.Linux.Navigation;

public interface INavigationService
{
	void NavigateTo(string route);
	bool CanGoBack { get; }
	void GoBack();
}
