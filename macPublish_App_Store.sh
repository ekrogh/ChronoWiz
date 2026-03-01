
#!/bin/bash
set -euo pipefail

PROFILE_NAME="ChronoWiz_mac_app_store"
PROFILE_DIR="$HOME/Library/MobileDevice/Provisioning Profiles"
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

find_profile_uuid() {
	python3 - "$1" "$2" <<'PY'
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
}

PROFILE_UUID="$(find_profile_uuid "$PROFILE_NAME" "$PROFILE_DIR")"

if [[ -z "$PROFILE_UUID" ]]; then
	PROFILE_SOURCE_CANDIDATES=(
		"${PROFILE_SOURCE_OVERRIDE:-}"
		"$SCRIPT_DIR/../../Certificates/ChronoWiz_Mac_App_Store.provisionprofile"
		"$SCRIPT_DIR/Certificates/ChronoWiz_Mac_App_Store.provisionprofile"
	)

	for SOURCE_PROFILE in "${PROFILE_SOURCE_CANDIDATES[@]}"; do
		[[ -n "$SOURCE_PROFILE" && -f "$SOURCE_PROFILE" ]] || continue

		SOURCE_NAME="$(security cms -D -i "$SOURCE_PROFILE" | plutil -extract Name raw -o - - 2>/dev/null || true)"
		[[ "$SOURCE_NAME" == "$PROFILE_NAME" ]] || continue

		SOURCE_UUID="$(security cms -D -i "$SOURCE_PROFILE" | plutil -extract UUID raw -o - - 2>/dev/null || true)"
		[[ -n "$SOURCE_UUID" ]] || continue

		mkdir -p "$PROFILE_DIR"
		cp -f "$SOURCE_PROFILE" "$PROFILE_DIR/$SOURCE_UUID.provisionprofile"

		PROFILE_UUID="$(find_profile_uuid "$PROFILE_NAME" "$PROFILE_DIR")"
		if [[ -n "$PROFILE_UUID" ]]; then
			echo "Imported provisioning profile '$PROFILE_NAME' from $SOURCE_PROFILE"
			break
		fi
	done
fi

if [[ -z "$PROFILE_UUID" ]]; then
	echo "ERROR: Provisioning profile '$PROFILE_NAME' was not found in $PROFILE_DIR"
	echo "Tried importing from:"
	echo "  - $SCRIPT_DIR/../../Certificates/ChronoWiz_Mac_App_Store.provisionprofile"
	echo "  - $SCRIPT_DIR/Certificates/ChronoWiz_Mac_App_Store.provisionprofile"
	echo "  - \\$PROFILE_SOURCE_OVERRIDE (if set)"
	echo "Install a valid Mac App Store provisioning profile for dk.eksit.chronowiz before publishing."
	exit 1
fi

echo "Using provisioning profile '$PROFILE_NAME' (UUID: $PROFILE_UUID)"

dotnet build -t:Clean
dotnet clean
rm -rf /Users/eks/Downloads/ChronoWiz*.pkg
dotnet publish ChronoWiz/ChronoWiz.csproj -f net10.0-maccatalyst -c Release -p:MtouchLink=SdkOnly -p:ArchiveOnBuild=true -p:CreatePackage=true -p:EnableCodeSigning=true -p:EnablePackageSigning=true -p:CodesignKey="Apple Distribution: Eigil Krogh (4657Q2Y6NH)" -p:CodesignProvision="$PROFILE_UUID" -p:CodesignEntitlements="Platforms/MacCatalyst/Entitlements.Release.plist" -p:MtouchExtraArgs="--nowarn:7151" -p:PackageSigningKey="3rd Party Mac Developer Installer: Eigil Krogh (4657Q2Y6NH)" -o "/Users/eks/Downloads/"
