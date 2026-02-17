#!/bin/bash
# Script to update all .NET SDK installations on macOS
# This script will download and install the latest .NET SDK versions

set -euo pipefail

# Color codes for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

echo -e "${BLUE}========================================${NC}"
echo -e "${BLUE}.NET SDK Update Script for macOS${NC}"
echo -e "${BLUE}========================================${NC}"
echo ""

# Check if running on macOS
if [[ "$OSTYPE" != "darwin"* ]]; then
    echo -e "${RED}ERROR: This script is designed for macOS only.${NC}"
    echo -e "${RED}Current OS: $OSTYPE${NC}"
    exit 1
fi

# Check if dotnet is installed
if ! command -v dotnet &> /dev/null; then
    echo -e "${RED}ERROR: dotnet is not installed or not in PATH.${NC}"
    echo -e "${YELLOW}Please install .NET SDK from: https://dotnet.microsoft.com/download${NC}"
    exit 1
fi

# Function to get the architecture
get_architecture() {
    local arch=$(uname -m)
    if [[ "$arch" == "arm64" ]]; then
        echo "arm64"
    elif [[ "$arch" == "x86_64" ]]; then
        echo "x64"
    else
        echo -e "${RED}Unsupported architecture: $arch${NC}"
        exit 1
    fi
}

ARCH=$(get_architecture)
echo -e "${GREEN}Detected architecture: $ARCH${NC}"
echo ""

# Display current installed SDKs
echo -e "${YELLOW}Current installed .NET SDKs:${NC}"
dotnet --list-sdks
echo ""

# Display current installed runtimes
echo -e "${YELLOW}Current installed .NET Runtimes:${NC}"
dotnet --list-runtimes
echo ""

# Function to download and install .NET SDK
install_dotnet_sdk() {
    local version=$1
    local channel=$2
    
    echo -e "${BLUE}Installing .NET SDK $version ($channel channel)...${NC}"
    
    # Download the install script
    local install_script="/tmp/dotnet-install-${version}.sh"
    
    if ! curl -sSL https://dot.net/v1/dotnet-install.sh -o "$install_script"; then
        echo -e "${RED}Failed to download .NET install script${NC}"
        return 1
    fi
    
    chmod +x "$install_script"
    
    # Install the SDK
    if [[ "$version" == "latest" ]]; then
        if ! "$install_script" --channel "$channel" --architecture "$ARCH" --install-dir /usr/local/share/dotnet; then
            echo -e "${RED}Failed to install .NET SDK from $channel channel${NC}"
            rm -f "$install_script"
            return 1
        fi
    else
        if ! "$install_script" --version "$version" --architecture "$ARCH" --install-dir /usr/local/share/dotnet; then
            echo -e "${RED}Failed to install .NET SDK version $version${NC}"
            rm -f "$install_script"
            return 1
        fi
    fi
    
    rm -f "$install_script"
    echo -e "${GREEN}Successfully installed .NET SDK from $channel channel${NC}"
    echo ""
}

# Ask user what they want to update
echo -e "${YELLOW}What would you like to do?${NC}"
echo "1) Install/Update latest .NET 10.0 (LTS)"
echo "2) Install/Update latest .NET 9.0 (STS)"
echo "3) Install/Update latest .NET 8.0 (LTS)"
echo "4) Install/Update all stable channels (8.0, 9.0, 10.0)"
echo "5) Exit"
echo ""
read -p "Enter your choice (1-5): " choice

case $choice in
    1)
        install_dotnet_sdk "latest" "10.0"
        ;;
    2)
        install_dotnet_sdk "latest" "9.0"
        ;;
    3)
        install_dotnet_sdk "latest" "8.0"
        ;;
    4)
        install_dotnet_sdk "latest" "8.0"
        install_dotnet_sdk "latest" "9.0"
        install_dotnet_sdk "latest" "10.0"
        ;;
    5)
        echo -e "${BLUE}Exiting...${NC}"
        exit 0
        ;;
    *)
        echo -e "${RED}Invalid choice. Exiting.${NC}"
        exit 1
        ;;
esac

# Display updated installed SDKs
echo -e "${GREEN}========================================${NC}"
echo -e "${GREEN}Updated .NET SDKs:${NC}"
echo -e "${GREEN}========================================${NC}"
dotnet --list-sdks
echo ""

echo -e "${GREEN}========================================${NC}"
echo -e "${GREEN}Updated .NET Runtimes:${NC}"
echo -e "${GREEN}========================================${NC}"
dotnet --list-runtimes
echo ""

echo -e "${GREEN}✓ Update process completed successfully!${NC}"
echo ""
echo -e "${YELLOW}Note: You may need to restart your terminal or run 'source ~/.zshrc' or 'source ~/.bash_profile'${NC}"
echo -e "${YELLOW}to ensure the updated .NET is in your PATH.${NC}"
