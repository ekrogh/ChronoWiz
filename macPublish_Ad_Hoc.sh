dotnet build -t:Clean
dotnet clean
rm -rf /Users/eks/Downloads/ChronoWiz*.pkg
dotnet publish ChronoWiz/ChronoWiz.csproj -f net8.0-maccatalyst -c Release -p:MtouchLink=SdkOnly -p:CreatePackage=true -p:EnableCodeSigning=true  -p:CodesignKey="Apple Development: Eigil Krogh (M39X2ZPF3C)" -p:CodesignProvision="ChronoWiz Dvl MacCatalyst" -p:CodesignEntitlements="Platforms/MacCatalyst/Entitlements.Release.plist" -p:UseHardenedRuntime=true -o "/Users/eks/Downloads/"
