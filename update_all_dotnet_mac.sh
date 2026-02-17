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
  mapfile -t DOTNET_FORMULAE < <(brew list --formula | grep -E '^(dotnet|dotnet-sdk|dotnet-runtime|aspnetcore-runtime)(@.*)?$' || true)
  mapfile -t DOTNET_CASKS < <(brew list --cask | grep -E '^dotnet-sdk(@.*)?$' || true)

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
  dotnet workload update || true

  echo "==> Updating installed global .NET tools"
  mapfile -t GLOBAL_TOOLS < <(dotnet tool list -g 2>/dev/null | awk 'NR>2 && NF>0 {print $1}' || true)
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
