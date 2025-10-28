# Pipeline Troubleshooting Guide

## Common Issues and Solutions

### 1. "No data was written into the file" Error

This error typically occurs when:
- Build tasks don't produce expected output files
- File paths are incorrect
- Build artifacts are not properly generated

**Solutions:**
1. Ensure all build steps complete successfully
2. Check that publish output directory exists and contains files
3. Verify file paths in pipeline tasks
4. Use the provided build scripts to test locally first

### 2. Missing Build Artifacts

**Check:**
- Build configuration is set to Release
- Publish output directory is correct
- All required files are included in artifacts

**Test locally:**
```powershell
# Windows
.\build.ps1 -Clean -Test

# Linux/Mac
./build.sh Release true true
```

### 3. Docker Build Issues

**Common problems:**
- Dockerfile syntax errors
- Missing dependencies
- Incorrect base image

**Test Docker build locally:**
```bash
docker build -t quizapp .
docker run -p 5000:80 quizapp
```

### 4. Pipeline Configuration

**Azure DevOps:**
- Use the provided `azure-pipelines.yml`
- Ensure service connections are configured
- Check build agent capabilities

**GitHub Actions:**
- Use the provided `.github/workflows/ci-cd.yml`
- Configure secrets for Docker Hub (if needed)
- Check repository permissions

### 5. Build Scripts

**PowerShell (Windows):**
```powershell
.\build.ps1 -Clean -Test -Docker
```

**Bash (Linux/Mac):**
```bash
./build.sh Release true true true
```

### 6. Verification Steps

1. **Local Build Test:**
   ```bash
   dotnet restore
   dotnet build --configuration Release
   dotnet publish --configuration Release --output ./publish
   ```

2. **Docker Test:**
   ```bash
   docker build -t quizapp .
   docker run -d -p 5000:80 quizapp
   curl http://localhost:5000
   ```

3. **Pipeline Logs:**
   - Check build logs for specific error messages
   - Verify all tasks complete successfully
   - Check artifact publishing steps

### 7. File Structure

Ensure your project has:
```
QuizApp/
├── QuizApp.csproj
├── Program.cs
├── Dockerfile
├── .dockerignore
├── azure-pipelines.yml
├── .github/workflows/ci-cd.yml
├── build.ps1
├── build.sh
└── Pages/
    └── ...
```

### 8. Environment Variables

**Azure DevOps:**
- `buildConfiguration`: Release
- `dockerRegistryServiceConnection`: Your Docker registry connection
- `imageRepository`: quizapp

**GitHub Actions:**
- `DOTNET_VERSION`: 8.0.x
- `DOCKER_IMAGE_NAME`: quizapp
- `DOCKER_TAG`: ${{ github.sha }}

### 9. Common Fixes

1. **Add explicit file copying:**
   ```yaml
   - task: CopyFiles@2
     inputs:
       SourceFolder: '$(Build.SourcesDirectory)'
       Contents: '**/*'
       TargetFolder: '$(Build.ArtifactStagingDirectory)'
   ```

2. **Ensure publish output:**
   ```yaml
   - task: DotNetCoreCLI@2
     inputs:
       command: 'publish'
       publishWebProjects: false
       projects: '**/*.csproj'
       arguments: '--configuration $(buildConfiguration) --output $(Build.ArtifactStagingDirectory)'
   ```

3. **Add validation steps:**
   ```yaml
   - task: PowerShell@2
     inputs:
       script: |
         if (Test-Path "$(Build.ArtifactStagingDirectory)") {
           Write-Host "Artifacts directory exists"
           Get-ChildItem "$(Build.ArtifactStagingDirectory)" -Recurse
         } else {
           Write-Error "Artifacts directory not found"
           exit 1
         }
   ```

