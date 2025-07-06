# .NET 9.0 Upgrade Plan

## Execution Steps

1. Validate that an .NET 9.0 SDK required for this upgrade is installed on the machine and if not, help to get it installed.
2. Ensure that the SDK version specified in global.json files is compatible with the .NET 9.0 upgrade.
3. Upgrade ChronoWiz\ChronoWiz.csproj

## Settings

This section contains settings and data used by execution steps.

### Excluded projects

| Project name                                   | Description                 |
|:-----------------------------------------------|:---------------------------:|

### Aggregate NuGet packages modifications across all projects

| Package Name                        | Current Version | New Version | Description                         |
|:------------------------------------|:---------------:|:-----------:|:------------------------------------|
| Microsoft.Extensions.Logging.Debug  |   8.0.0         |  9.0.6      | Recommended for .NET 9.0            |

### Project upgrade details

#### ChronoWiz\ChronoWiz.csproj modifications

Project properties changes:
  - Target frameworks should be changed from `net8.0-ios;net8.0-maccatalyst;net8.0-android34.0;net8.0-windows10.0.22621.0` to `net9.0-windows`

NuGet packages changes:
  - Microsoft.Extensions.Logging.Debug should be updated from `8.0.0` to `9.0.6` (*recommended for .NET 9.0*)

Other changes:
  - None
