#!/bin/bash
# Automated script to update all .NET SDK installations on macOS
# This script will automatically update all stable .NET channels without prompting

set -euo pipefail

# Color codes for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

echo -e "${BLUE}========================================${NC}"
echo -e "${BLUE}.NET SDK Auto-Update Script for macOS${NC}"
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

# Function to download and install .NET SDK
install_dotnet_sdk() {
    local channel=$1
    
    echo -e "${BLUE}Installing latest .NET SDK from $channel channel...${NC}"
    
    # Download the install script
    local install_script="/tmp/dotnet-install-${channel}.sh"
    
    if ! curl -sSL https://dot.net/v1/dotnet-install.sh -o "$install_script"; then
        echo -e "${RED}Failed to download .NET install script${NC}"
        return 1
    fi
    
    chmod +x "$install_script"
    
    # Install the SDK
    if ! "$install_script" --channel "$channel" --architecture "$ARCH" --install-dir /usr/local/share/dotnet; then
        echo -e "${RED}Failed to install .NET SDK from $channel channel${NC}"
        rm -f "$install_script"
        return 1
    fi
    
    rm -f "$install_script"
    echo -e "${GREEN}Successfully installed .NET SDK from $channel channel${NC}"
    echo ""
}

# Update all stable channels
echo -e "${BLUE}Updating all stable .NET channels...${NC}"
echo ""

install_dotnet_sdk "8.0"  # LTS
install_dotnet_sdk "9.0"  # STS
install_dotnet_sdk "10.0" # LTS

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

echo -e "${GREEN}✓ All .NET SDKs updated successfully!${NC}"
echo ""
echo -e "${YELLOW}Note: You may need to restart your terminal or run 'source ~/.zshrc' or 'source ~/.bash_profile'${NC}"
echo -e "${YELLOW}to ensure the updated .NET is in your PATH.${NC}"
