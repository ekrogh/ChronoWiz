
#!/bin/bash
set -euo pipefail

PROFILE_NAME="ChronoWiz_Mac_App_Store"
PROFILE_DIR="$HOME/Library/MobileDevice/Provisioning Profiles"

if ! grep -l "$PROFILE_NAME" "$PROFILE_DIR"/*.provisionprofile >/dev/null 2>&1; then
	echo "ERROR: Provisioning profile '$PROFILE_NAME' was not found in $PROFILE_DIR"
	echo "Install a valid Mac App Store provisioning profile for dk.eksit.chronowiz before publishing."
	exit 1
fi

dotnet build -t:Clean
dotnet clean
rm -rf /Users/eks/Downloads/ChronoWiz*.pkg
dotnet publish ChronoWiz/ChronoWiz.csproj -f net10.0-maccatalyst -c Release -p:MtouchLink=SdkOnly -p:CreatePackage=true -p:EnableCodeSigning=true -p:EnablePackageSigning=true -p:CodesignKey="Apple Distribution: Eigil Krogh (4657Q2Y6NH)" -p:CodesignProvision="$PROFILE_NAME" -p:CodesignEntitlements="Platforms/MacCatalyst/Entitlements.Release.plist" -p:PackageSigningKey="3rd Party Mac Developer Installer: Eigil Krogh (4657Q2Y6NH)" -o "/Users/eks/Downloads/"
