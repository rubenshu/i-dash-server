# PowerShell script to generate HTML coverage report from latest coverage.cobertura.xml

$ErrorActionPreference = 'Stop'

# Run tests with coverage for both test projects
Write-Host "Running tests with coverage for ItemDashServer.Api.Tests..."
dotnet test ./tests/ItemDashServer.Api.Tests/ItemDashServer.Api.Tests.csproj --collect:"XPlat Code Coverage"
Write-Host "Running tests with coverage for ItemDashServer.Application.Tests..."
dotnet test ./tests/ItemDashServer.Application.Tests/ItemDashServer.Application.Tests.csproj --collect:"XPlat Code Coverage"
Write-Host "Running tests with coverage for ItemDashServer.Infrastructure.Tests..."
dotnet test ./tests/ItemDashServer.Infrastructure.Tests/ItemDashServer.Infrastructure.Tests.csproj --collect:"XPlat Code Coverage"

# Find the latest coverage.cobertura.xml file in all bin/Debug/net8.0 folders
$testResults = Get-ChildItem -Path @("./tests/ItemDashServer.Api.Tests/", "./tests/ItemDashServer.Application.Tests/", "./tests/ItemDashServer.Infrastructure.Tests/") -Recurse -Filter coverage.cobertura.xml | Sort-Object LastWriteTime -Descending
if ($testResults.Count -eq 0) {
    Write-Error "No coverage.cobertura.xml file found. Run your tests with coverage first."
    exit 1
}
$coverageFile = $testResults[0].FullName

# Ensure reportgenerator is installed
if (-not (Get-Command reportgenerator -ErrorAction SilentlyContinue)) {
    Write-Host "Installing reportgenerator..."
    dotnet tool install -g dotnet-reportgenerator-globaltool
    $env:PATH += ";$env:USERPROFILE\.dotnet\tools"
}

# Generate the HTML report with assembly filter for main projects
$targetDir = ".\tests\coveragereport"
reportgenerator -reports:$coverageFile -targetdir:$targetDir -reporttypes:Html -assemblyfilters:+ItemDashServer.* -classfilters:"-*.Dto;-*Dto;-*MappingProfile"
Write-Host "Coverage report generated at $targetDir\index.html"
