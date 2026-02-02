using ChronoWiz.Ui.ICSFiling;

namespace ChronoWiz.Ui.View;

public partial class LinuxMainPage : ContentPage
{
	public LinuxMainPage()
	{
		InitializeComponent();
		ResultLabel.Text = $"Started at: {DateTime.Now}";
	}

	private async void OnHelpClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(AboutHelp.AboutHelp), true);
	}
}
