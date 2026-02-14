
#!/bin/bash
set -euo pipefail

PROFILE_NAME="ChronoWiz_mac_app_store"
PROFILE_DIR="$HOME/Library/MobileDevice/Provisioning Profiles"

PROFILE_UUID="$(python3 - "$PROFILE_NAME" "$PROFILE_DIR" <<'PY'
import os
import plistlib
import subprocess
import sys

target_name = sys.argv[1]
profile_dir = os.path.expanduser(sys.argv[2])

if not os.path.isdir(profile_dir):
	sys.exit(0)

for entry in os.listdir(profile_dir):
	if not entry.endswith('.provisionprofile'):
		continue
	path = os.path.join(profile_dir, entry)
	try:
		decoded = subprocess.check_output(["security", "cms", "-D", "-i", path])
		profile = plistlib.loads(decoded)
	except Exception:
		continue

	if profile.get("Name") == target_name:
		print(profile.get("UUID", ""))
		break
PY
)"

if [[ -z "$PROFILE_UUID" ]]; then
	echo "ERROR: Provisioning profile '$PROFILE_NAME' was not found in $PROFILE_DIR"
	echo "Install a valid Mac App Store provisioning profile for dk.eksit.chronowiz before publishing."
	exit 1
fi

echo "Using provisioning profile '$PROFILE_NAME' (UUID: $PROFILE_UUID)"

dotnet build -t:Clean
dotnet clean
rm -rf /Users/eks/Downloads/ChronoWiz*.pkg
dotnet publish ChronoWiz/ChronoWiz.csproj -f net10.0-maccatalyst -c Release -p:MtouchLink=SdkOnly -p:CreatePackage=true -p:EnableCodeSigning=true -p:EnablePackageSigning=true -p:CodesignKey="Apple Distribution: Eigil Krogh (4657Q2Y6NH)" -p:CodesignProvision="$PROFILE_UUID" -p:CodesignEntitlements="Platforms/MacCatalyst/Entitlements.Release.plist" -p:PackageSigningKey="3rd Party Mac Developer Installer: Eigil Krogh (4657Q2Y6NH)" -o "/Users/eks/Downloads/"
