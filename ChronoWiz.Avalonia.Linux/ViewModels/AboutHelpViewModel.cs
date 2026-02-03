using ChronoWiz.Avalonia.Linux.Services;
using ChronoWiz.Shared;
using System.Reflection;
using System.Windows.Input;

namespace ChronoWiz.Avalonia.Linux.ViewModels;

public sealed class AboutHelpViewModel : ViewModelBase
{
    private readonly ILinkOpener _linkOpener;
    private readonly string _appName;
    private readonly string _appVersion;

    public string Title => "Help";

    public string AppInfoText => $"{_appName}  Version: {_appVersion}";

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

        var asm = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
        _appName = asm.GetName().Name ?? "ChronoWiz";
        //_appVersion =
        //    asm.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion
        //    ?? asm.GetName().Version?.ToString()
        //    ?? "";
        _appVersion = Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? "";
        OpenUsersGuideCommand = new RelayCommand(() => _ = _linkOpener.OpenAsync(Links.UsersGuide));
        OpenHomePageCommand = new RelayCommand(() => _ = _linkOpener.OpenAsync(Links.HomePage));
        OpenEmailCommand = new RelayCommand(() => _ = _linkOpener.OpenAsync(Links.MailTo));
        GoBackCommand = new RelayCommand(() => Navigation?.GoBack(), () => Navigation?.CanGoBack ?? false);
    }
}
