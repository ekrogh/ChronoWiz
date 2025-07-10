dotnet build -t:Clean
dotnet clean
dotnet publish ChronoWiz/ChronoWiz.csproj -f net9.0-maccatalyst -c Release -p:MtouchLink=SdkOnly -p:CreatePackage=true -p:EnableCodeSigning=true  -p:CodesignKey="Apple Distribution: Eigil Krogh (4657Q2Y6NH)" -p:CodesignProvision="ChronoWiz Dvl MacCatalyst" -p:CodesignEntitlements="Platforms/MacCatalyst/Entitlements.Release.plist" -p:UseHardenedRuntime=true -o ".\Packs"

@REM dotnet build -t:Clean
@REM dotnet clean
@REM dotnet publish ChronoWiz\ChronoWiz.csproj -f net9.0-maccatalyst -c Release -p:MtouchLink=SdkOnly -p:CreatePackage=true -p:EnableCodeSigning=true  -p:CodesignKey="Apple Distribution: Eigil Krogh (4657Q2Y6NH)" -p:CodesignProvision="ChronoWiz_Mac_App_Store" -p:CodesignEntitlements="Platforms\MacCatalyst\Entitlements.Release.plist" -p:UseHardenedRuntime=true -o "d:\Users\eigil\Downloads\"