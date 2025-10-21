# PowerShell build script for QuizApp
param(
    [string]$Configuration = "Release",
    [switch]$Clean,
    [switch]$Test,
    [switch]$Docker
)

Write-Host "Starting build process for QuizApp..." -ForegroundColor Green

if ($Clean) {
    Write-Host "Cleaning previous build artifacts..." -ForegroundColor Yellow
    dotnet clean QuizApp.csproj --configuration $Configuration
    Remove-Item -Recurse -Force -ErrorAction SilentlyContinue bin, obj
}

Write-Host "Restoring NuGet packages..." -ForegroundColor Yellow
dotnet restore QuizApp.csproj

if ($LASTEXITCODE -ne 0) {
    Write-Error "Package restore failed!"
    exit 1
}

Write-Host "Building project..." -ForegroundColor Yellow
dotnet build QuizApp.csproj --configuration $Configuration --no-restore

if ($LASTEXITCODE -ne 0) {
    Write-Error "Build failed!"
    exit 1
}

if ($Test) {
    Write-Host "Running tests..." -ForegroundColor Yellow
    dotnet test QuizApp.csproj --configuration $Configuration --no-build --collect:"XPlat Code Coverage" --logger trx --results-directory ./TestResults
}

Write-Host "Publishing application..." -ForegroundColor Yellow
dotnet publish QuizApp.csproj --configuration $Configuration --output ./publish --no-build

if ($LASTEXITCODE -ne 0) {
    Write-Error "Publish failed!"
    exit 1
}

if ($Docker) {
    Write-Host "Building Docker image..." -ForegroundColor Yellow
    docker build -t quizapp:latest .
    
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Docker build failed!"
        exit 1
    }
}

Write-Host "Build completed successfully!" -ForegroundColor Green
