dotnet build -t:Clean
dotnet clean
dotnet publish ChronoWiz/ChronoWiz.csproj -f net9.0-maccatalyst -c Release -p:MtouchLink=SdkOnly -p:EnableCodeSigning=true  -p:CodesignKey="Apple Development: Created via API (K928FRRPW3)" -p:CodesignProvision="ChronoWiz_Mac_App_Store" -p:CodesignEntitlements="Platforms/MacCatalyst/Entitlements.Release.plist" -p:UseHardenedRuntime=true 
