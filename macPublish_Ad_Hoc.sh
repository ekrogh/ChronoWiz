dotnet build -t:Clean
dotnet clean
rm -rf /Users/eks/Downloads/ChronoWiz*.pkg
dotnet publish ChronoWiz/ChronoWiz.csproj -f net9.0-maccatalyst -c Release -p:MtouchLink=SdkOnly -p:CreatePackage=true -p:EnableCodeSigning=true  -p:CodesignKey="Apple Development: Eigil Krogh (M39X2ZPF3C)" -p:CodesignProvision="Chronowiz_macOS_App_Development" -p:CodesignEntitlements="Platforms/MacCatalyst/Entitlements.Release.plist" -p:UseHardenedRuntime=true -o "/Users/eks/Downloads/"
