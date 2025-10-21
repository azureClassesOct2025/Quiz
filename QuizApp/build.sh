#!/bin/bash

# Bash build script for QuizApp
set -e

CONFIGURATION=${1:-Release}
CLEAN=${2:-false}
TEST=${3:-false}
DOCKER=${4:-false}

echo "Starting build process for QuizApp..."

if [ "$CLEAN" = "true" ]; then
    echo "Cleaning previous build artifacts..."
    dotnet clean QuizApp.csproj --configuration $CONFIGURATION
    rm -rf bin obj
fi

echo "Restoring NuGet packages..."
dotnet restore QuizApp.csproj

echo "Building project..."
dotnet build QuizApp.csproj --configuration $CONFIGURATION --no-restore

if [ "$TEST" = "true" ]; then
    echo "Running tests..."
    dotnet test QuizApp.csproj --configuration $CONFIGURATION --no-build --collect:"XPlat Code Coverage" --logger trx --results-directory ./TestResults
fi

echo "Publishing application..."
dotnet publish QuizApp.csproj --configuration $CONFIGURATION --output ./publish --no-build

if [ "$DOCKER" = "true" ]; then
    echo "Building Docker image..."
    docker build -t quizapp:latest .
fi

echo "Build completed successfully!"
