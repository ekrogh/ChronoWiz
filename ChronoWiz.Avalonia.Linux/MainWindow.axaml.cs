using Avalonia.Controls;
using ChronoWiz.Avalonia.Linux.ViewModels;
using ChronoWiz.Avalonia.Linux.Services;

namespace ChronoWiz.Avalonia.Linux;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
		var vm = new MainWindowViewModel();
		var ics = new AvaloniaIcsFileService(this);
		vm.PickAndReadIcsAsync = correct => ics.PickAndReadIcsAsync(correct);
		vm.PickAndSaveIcsAsync = content => ics.PickAndSaveIcsAsync(content);
		DataContext = vm;
    }
}