dotnet build -t:Clean
dotnet clean
dotnet publish ChronoWiz\ChronoWiz.csproj -f net10.0-maccatalyst -c Release --sc  -o ".\Pack" -p:MtouchLink=SdkOnly -p:CreatePackage=true -p:EnableCodeSigning=true -p:EnablePackageSigning=true -p:CodesignKey="Developer ID Application: Eigil Krogh (4657Q2Y6NH)" -p:CodesignProvision="ChronoWiz_Developer_ID_Application" -p:CodesignEntitlements="Platforms\MacCatalyst\Entitlements.Release.plist" -p:PackageSigningKey="Developer ID Installer: Eigil Krogh (4657Q2Y6NH)"


@REM dotnet publish ChronoWiz/ChronoWiz.csproj -f net10.0-maccatalyst -c Release -p:MtouchLink=SdkOnly -p:CreatePackage=true -p:EnableCodeSigning=true -p:EnablePackageSigning=true -p:CodesignKey="Developer ID Application: Eigil Krogh (4657Q2Y6NH)" -p:CodesignProvision="ChronoWiz_Developer_ID_Application" -p:CodesignEntitlements="Platforms/MacCatalyst/Entitlements.Release.plist" -p:PackageSigningKey="Developer ID Installer: Eigil Krogh (4657Q2Y6NH)" -o "/Users/eks/Downloads/"

@REM dotnet build -t:Clean
@REM dotnet clean
