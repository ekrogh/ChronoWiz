using ChronoWiz.Avalonia.Linux.Services;
using ChronoWiz.Shared;
using System;
using System.Windows.Input;

namespace ChronoWiz.Avalonia.Linux.ViewModels;

public sealed class AboutHelpViewModel : ViewModelBase
{
	private readonly ILinkOpener _linkOpener;

	public string Title => "Help";

	public string AppInfoText => "ChronoWiz\nLinux UI powered by Avalonia";

	public string Author => "Eigil Krogh Sorensen";

	public string Url => Links.HomePage.ToString();

	public string Email => "eks@eksit.dk";

	public ICommand OpenUsersGuideCommand { get; }
	public ICommand OpenHomePageCommand { get; }
	public ICommand OpenEmailCommand { get; }
	public ICommand GoBackCommand { get; }

	public AboutHelpViewModel()
		: this(new ProcessLinkOpener())
	{
	}

	public AboutHelpViewModel(ILinkOpener linkOpener)
	{
		_linkOpener = linkOpener;
		OpenUsersGuideCommand = new RelayCommand(() => _ = _linkOpener.OpenAsync(Links.UsersGuide));
		OpenHomePageCommand = new RelayCommand(() => _ = _linkOpener.OpenAsync(Links.HomePage));
		OpenEmailCommand = new RelayCommand(() => _ = _linkOpener.OpenAsync(Links.MailTo));
		GoBackCommand = new RelayCommand(() => Navigation?.GoBack(), () => Navigation?.CanGoBack ?? false);
	}
}
