#!/usr/bin/env bash
set -euo pipefail

if [[ "$(uname -s)" != "Darwin" ]]; then
  echo "This script is for macOS only."
  exit 1
fi

echo "==> Starting .NET update on macOS"

if command -v brew >/dev/null 2>&1; then
  echo "==> Updating Homebrew metadata"
  brew update

  echo "==> Finding installed Homebrew .NET packages"
  DOTNET_FORMULAE=()
  while IFS= read -r line; do
    DOTNET_FORMULAE+=("$line")
  done < <(brew list --formula | grep -E '^(dotnet|dotnet-sdk|dotnet-runtime|aspnetcore-runtime)(@.*)?$' || true)

  DOTNET_CASKS=()
  while IFS= read -r line; do
    DOTNET_CASKS+=("$line")
  done < <(brew list --cask | grep -E '^dotnet-sdk(@.*)?$' || true)

  if (( ${#DOTNET_FORMULAE[@]} > 0 )); then
    echo "==> Upgrading .NET formulae: ${DOTNET_FORMULAE[*]}"
    brew upgrade "${DOTNET_FORMULAE[@]}" || true
  else
    echo "==> No Homebrew .NET formulae installed"
  fi

  if (( ${#DOTNET_CASKS[@]} > 0 )); then
    echo "==> Upgrading .NET casks: ${DOTNET_CASKS[*]}"
    brew upgrade --cask "${DOTNET_CASKS[@]}" || true
  else
    echo "==> No Homebrew .NET casks installed"
  fi
else
  echo "==> Homebrew not found; skipping Homebrew package updates"
fi

if command -v dotnet >/dev/null 2>&1; then
  echo "==> Current SDKs before workload update"
  dotnet --list-sdks || true

  echo "==> Updating .NET workloads"
  if ! dotnet workload update; then
    echo "==> Workload update failed without elevated privileges"
    if command -v sudo >/dev/null 2>&1; then
      echo "==> Retrying workload update with sudo (may prompt for password)"
      sudo dotnet workload update || true
    else
      echo "==> sudo not available; skipping workload update"
    fi
  fi

  echo "==> Updating installed global .NET tools"
  GLOBAL_TOOLS=()
  while IFS= read -r line; do
    GLOBAL_TOOLS+=("$line")
  done < <(dotnet tool list -g 2>/dev/null | awk 'NR>2 && NF>0 {print $1}' || true)
  if (( ${#GLOBAL_TOOLS[@]} > 0 )); then
    for tool in "${GLOBAL_TOOLS[@]}"; do
      echo "   -> Updating $tool"
      dotnet tool update -g "$tool" || true
    done
  else
    echo "==> No global .NET tools installed"
  fi

  echo "==> SDKs after update"
  dotnet --list-sdks || true
  echo "==> Runtimes after update"
  dotnet --list-runtimes || true
else
  echo "==> dotnet command not found on PATH"
  echo "Install .NET SDK first: https://dotnet.microsoft.com/download"
  exit 1
fi

echo "==> .NET update script completed"
