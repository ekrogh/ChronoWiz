using System;
using TimeDateCalculator.Interfaces;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace TimeDateCalculatorDll
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AboutHelp : ContentPage
	{
		public AboutHelp()
		{
			InitializeComponent();

			// TODO Xamarin.Forms.Device.RuntimePlatform is no longer supported. Use Microsoft.Maui.Devices.DeviceInfo.Platform instead. For more details see https://learn.microsoft.com/en-us/dotnet/maui/migration/forms-projects#device-changes
			if (Device.RuntimePlatform == Device.Android)
			{
				AboutHelpContentPageName.SetAppThemeColor(ContentPage.BackgroundColorProperty, Colors.White, Colors.Black);
				Resources["DynamicBaseButtonStyle"] = Resources["AndroidBaseButtonStyle"];
			}
			else
			{
				Resources["DynamicBaseButtonStyle"] = Resources["baseButtonStyle"];
			}

			AppNameAndVer.Text =
								//"\""
								DependencyService.Get<IAppVersion>().GetAppTitle()
								//+ "\""
								+ "  Version: "
								+ DependencyService.Get<IAppVersion>().GetVersion()
								+ DependencyService.Get<IAppVersion>().GetBuild()
								+ DependencyService.Get<IAppVersion>().GetRevision();
		}

		private async void OKButton_Clicked(object sender, EventArgs e)
		{
			_ = await Navigation.PopAsync(true);
		}

		private async void UsersGuideButton_Clicked(object sender, EventArgs e)
		{
			// TODO Xamarin.Forms.Device.RuntimePlatform is no longer supported. Use Microsoft.Maui.Devices.DeviceInfo.Platform instead. For more details see https://learn.microsoft.com/en-us/dotnet/maui/migration/forms-projects#device-changes
			//if (DeviceInfo.Platform == DevicePlatform.GTK)
			//{
			//	System.Diagnostics.Process.Start("http://eksit.dk/users-guide-2/");
			//}
			//else
			//{
				if (await Launcher.CanOpenAsync(new Uri("http://eksit.dk/users-guide-2/")))
				{
					await Launcher.OpenAsync(new Uri("http://eksit.dk/users-guide-2/"));
				}
			//}
		}

		private async void MyUrlButton_Clicked(object sender, EventArgs e)
		{
			// TODO Xamarin.Forms.Device.RuntimePlatform is no longer supported. Use Microsoft.Maui.Devices.DeviceInfo.Platform instead. For more details see https://learn.microsoft.com/en-us/dotnet/maui/migration/forms-projects#device-changes
			//if (DeviceInfo.Platform == DevicePlatform.GTK)
			//{
			//	System.Diagnostics.Process.Start("http://eksit.dk/");
			//}
			//else
			//{
				if (await Launcher.CanOpenAsync(new Uri("http://eksit.dk/")))
				{
					await Launcher.OpenAsync(new Uri("http://eksit.dk/"));
				}
			//}
		}

		private async void EmaiBtn_Clicked(object sender, EventArgs e)
		{
			// TODO Xamarin.Forms.Device.RuntimePlatform is no longer supported. Use Microsoft.Maui.Devices.DeviceInfo.Platform instead. For more details see https://learn.microsoft.com/en-us/dotnet/maui/migration/forms-projects#device-changes
			//if (DeviceInfo.Platform == DevicePlatform.GTK)
			//{
			//	System.Diagnostics.Process.Start("mailto://timedatecalculator@eksit.dk");
			//}
			//else
			//{
				if (await Launcher.CanOpenAsync(new Uri("mailto://timedatecalculator@eksit.dk")))
				{
					await Launcher.OpenAsync(new Uri("mailto://timedatecalculator@eksit.dk"));
				}
			//}
		}
	}
}