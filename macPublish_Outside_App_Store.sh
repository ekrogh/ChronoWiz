dotnet build -t:Clean
dotnet clean
rm -rf /Users/eks/Downloads/ChronoWiz*.pkg
dotnet publish ChronoWiz/ChronoWiz.csproj -f net9.0-maccatalyst -c Release -p:MtouchLink=SdkOnly -p:CreatePackage=true -p:EnableCodeSigning=true -p:EnablePackageSigning=true -p:CodesignKey="Developer ID Application: Eigil Krogh (4657Q2Y6NH)" -p:CodesignProvision="ChronoWiz_Developer_ID_Application" -p:CodesignEntitlements="Platforms/MacCatalyst/Entitlements.Release.plist" -p:PackageSigningKey="Developer ID Installer: Eigil Krogh (4657Q2Y6NH)" -o "/Users/eks/Downloads/"
