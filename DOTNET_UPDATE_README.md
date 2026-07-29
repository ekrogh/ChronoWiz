# .NET Update Scripts for macOS

This directory contains shell scripts for updating .NET SDK installations on macOS.

## Scripts

### 1. `update_dotnet.sh` (Interactive)
Interactive script that allows you to choose which .NET versions to update.

**Usage:**
```bash
./update_dotnet.sh
```

**Features:**
- Detects system architecture (ARM64 or x64)
- Shows current installed .NET SDKs and runtimes
- Provides menu options:
  1. Install/Update latest .NET 10.0 (LTS)
  2. Install/Update latest .NET 9.0 (STS)
  3. Install/Update latest .NET 8.0 (LTS)
  4. Install/Update all stable channels (8.0, 9.0, 10.0)
  5. Exit
- Color-coded output for better readability
- Error handling and validation

### 2. `update_dotnet_auto.sh` (Automated)
Automated script that updates all stable .NET channels without user interaction.

**Usage:**
```bash
./update_dotnet_auto.sh
```

**Features:**
- Automatically updates .NET 8.0, 9.0, and 10.0
- No user interaction required
- Ideal for CI/CD pipelines or automated maintenance
- Shows before and after SDK/runtime versions

## Prerequisites

- macOS (the scripts check for this and will exit on other platforms)
- Existing .NET installation (or the scripts will provide download instructions)
- Internet connection for downloading updates
- Sufficient permissions (may require `sudo` for system-wide installations)

## Installation Locations

The scripts install .NET SDKs to `/usr/local/share/dotnet` by default. This is the standard location for user-installed .NET versions on macOS.

If you need to install to the system location (`/usr/share/dotnet`), you'll need to run the scripts with `sudo`:

```bash
sudo ./update_dotnet.sh
# or
sudo ./update_dotnet_auto.sh
```

## What Gets Updated

- **.NET 8.0**: Long-Term Support (LTS) version
- **.NET 9.0**: Standard-Term Support (STS) version
- **.NET 10.0**: Long-Term Support (LTS) version

The scripts install the latest versions within each channel, which includes:
- .NET SDK
- .NET Runtime
- ASP.NET Core Runtime

## Post-Update Steps

After running the update scripts, you may need to:

1. **Restart your terminal** or reload your shell configuration:
   ```bash
   # For zsh (default on modern macOS)
   source ~/.zshrc
   
   # For bash
   source ~/.bash_profile
   ```

2. **Verify the installation**:
   ```bash
   dotnet --version
   dotnet --list-sdks
   dotnet --list-runtimes
   ```

## Troubleshooting

### Permission Denied
If you get permission errors, try running with `sudo`:
```bash
sudo ./update_dotnet.sh
```

### Script Not Executable
Make the script executable:
```bash
chmod +x update_dotnet.sh
chmod +x update_dotnet_auto.sh
```

### .NET Not in PATH
Add .NET to your PATH by adding this to your `~/.zshrc` or `~/.bash_profile`:
```bash
export PATH="$PATH:/usr/local/share/dotnet"
```

### Download Failures
If downloads fail:
1. Check your internet connection
2. Verify that https://dot.net is accessible
3. Try running the script again

## Notes

- The scripts use the official Microsoft .NET installation script from `https://dot.net/v1/dotnet-install.sh`
- Previous versions are not removed automatically; you'll need to uninstall them manually if desired
- The scripts are designed specifically for macOS and will exit on other operating systems

## Manual .NET Management

To manually manage .NET versions:

### List Installed Versions
```bash
dotnet --list-sdks
dotnet --list-runtimes
```

### Uninstall a Version
Use the official uninstall script or manually remove from:
- `/usr/local/share/dotnet` (user installations)
- `/usr/share/dotnet` (system installations)

### Official Documentation
For more information, visit:
- [.NET Downloads](https://dotnet.microsoft.com/download)
- [.NET Installation Guide](https://docs.microsoft.com/dotnet/core/install/macos)
