# .NET8.0 Upgrade Plan

## Execution Steps

Execute steps below sequentially one by one in the order they are listed.

1. Validate that an .NET8.0 SDK required for this upgrade is installed on the machine and if not, help to get it installed.
2. Ensure that the SDK version specified in global.json files is compatible with the .NET8.0 upgrade.
3. Upgrade Fundo.Applications.WebApi\Fundo.Applications.WebApi.csproj
4. Upgrade Fundo.Services.Tests\Fundo.Services.Tests.csproj
5. Run unit tests to validate upgrade in the projects listed below:
 - Fundo.Services.Tests\Fundo.Services.Tests.csproj

## Settings

This section contains settings and data used by execution steps.

### Excluded projects

Table below contains projects that do belong to the dependency graph for selected projects and should not be included in the upgrade.

| Project name | Description |
|:-----------------------------------------------|:---------------------------:|

### Aggregate NuGet packages modifications across all projects

NuGet packages used across all selected projects or their dependencies that need version update in projects that reference them.

| Package Name | Current Version | New Version | Description |
|:------------------------------------|:---------------:|:-----------:|:----------------------------------------------|
| Microsoft.AspNetCore.Mvc.Testing |6.0.0 |8.0.22 | Recommended for .NET8 |
| Microsoft.AspNetCore.TestHost |6.0.20 |8.0.22 | Recommended for .NET8 |

### Project upgrade details
This section contains details about each project upgrade and modifications that need to be done in the project.

#### Fundo.Applications.WebApi\Fundo.Applications.WebApi.csproj modifications

Project properties changes:
 - Target framework should be changed from `net6.0` to `net8.0`

NuGet packages changes:
 - No changes required

Other changes:
 - None

#### Fundo.Services.Tests\Fundo.Services.Tests.csproj modifications

Project properties changes:
 - Target framework should be changed from `net6.0` to `net8.0`

NuGet packages changes:
 - Microsoft.AspNetCore.Mvc.Testing should be updated from `6.0.0` to `8.0.22` (recommended for .NET8)
 - Microsoft.AspNetCore.TestHost should be updated from `6.0.20` to `8.0.22` (recommended for .NET8)

Other changes:
 - None
