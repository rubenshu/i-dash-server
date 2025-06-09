# PowerShell script to generate HTML coverage report from latest coverage.cobertura.xml

dotnet clean --verbosity quiet
Get-ChildItem -Path . -Include bin,obj -Recurse -Directory | Remove-Item -Recurse -Force -ErrorAction SilentlyContinue

$ErrorActionPreference = 'Stop'

rmdir -Recurse -Force -Confirm:$false -ErrorAction SilentlyContinue ".\tests\ItemDashServer.Api.Tests\TestResults"
rmdir -Recurse -Force -Confirm:$false -ErrorAction SilentlyContinue ".\tests\ItemDashServer.Application.Tests\TestResults"
rmdir -Recurse -Force -Confirm:$false -ErrorAction SilentlyContinue ".\tests\ItemDashServer.Infrastructure.Tests\TestResults"
rmdir -Recurse -Force -Confirm:$false -ErrorAction SilentlyContinue ".\tests\coveragereport"

Write-Host "Running tests with coverage for ItemDashServer.Api.Tests..."
dotnet test ./tests/ItemDashServer.Api.Tests/ItemDashServer.Api.Tests.csproj --collect:"XPlat Code Coverage"
Write-Host "Running tests with coverage for ItemDashServer.Application.Tests..."
dotnet test ./tests/ItemDashServer.Application.Tests/ItemDashServer.Application.Tests.csproj --collect:"XPlat Code Coverage"
Write-Host "Running tests with coverage for ItemDashServer.Infrastructure.Tests..."
dotnet test ./tests/ItemDashServer.Infrastructure.Tests/ItemDashServer.Infrastructure.Tests.csproj --collect:"XPlat Code Coverage"

# Find all coverage.cobertura.xml files from all test projects
$coverageFiles = Get-ChildItem -Path @(
    "./tests/ItemDashServer.Api.Tests/",
    "./tests/ItemDashServer.Application.Tests/",
    "./tests/ItemDashServer.Infrastructure.Tests/"
) -Recurse -Filter coverage.cobertura.xml | Select-Object -ExpandProperty FullName

if ($coverageFiles.Count -eq 0) {
    Write-Error "No coverage.cobertura.xml file found. Run your tests with coverage first."
    exit 1
}
$coverageFilesArg = $coverageFiles -join ","

# Ensure reportgenerator is installed
if (-not (Get-Command reportgenerator -ErrorAction SilentlyContinue)) {
    Write-Host "Installing reportgenerator..."
    dotnet tool install -g dotnet-reportgenerator-globaltool
    $env:PATH += ";$env:USERPROFILE\.dotnet\tools"
}

# Generate the HTML report from all coverage files
$targetDir = ".\tests\coveragereport"
reportgenerator -reports:$coverageFilesArg -targetdir:$targetDir -reporttypes:Html  -assemblyfilters:+ItemDashServer.* -classfilters:"-*.Dto;-*Dto;-*MappingProfile"
Write-Host "Coverage report generated at $targetDir\index.html"
