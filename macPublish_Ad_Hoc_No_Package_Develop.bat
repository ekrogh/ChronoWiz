dotnet build -t:Clean
dotnet clean
dotnet publish ChronoWiz\ChronoWiz.csproj -f net9.0-maccatalyst -c Release -p:MtouchLink=SdkOnly -p:UseHardenedRuntime=true

@REM dotnet publish ChronoWiz\ChronoWiz.csproj -f net9.0-maccatalyst -c Release -p:MtouchLink=SdkOnly -p:EnableCodeSigning=true  -p:CodesignKey="Apple Development: Created via API (765QB69WBN)" -p:CodesignProvision="Chronowiz_macOS_App_Development" -p:CodesignEntitlements="Platforms\MacCatalyst\Entitlements.Release.plist" -p:UseHardenedRuntime=true