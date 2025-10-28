trigger:
- master

pool:
  vmImage: 'ubuntu-latest'

variables:
  imageName: 'quizapp'
  tag: '$(Build.BuildId)'

stages:
- stage: Build
  displayName: Build Docker Image
  jobs:
  - job: Build
    displayName: Build and Publish Docker Image
    steps:

    # 1️⃣ Checkout source
    - checkout: self

    # 2️⃣ Build the Docker image
    - task: Docker@2
      displayName: Build Docker Image
      inputs:
        command: build
        Dockerfile: '**/Dockerfile'
        tags: |
          $(imageName):$(tag)
    
    # 3️⃣ (Optional) Push to ACR or Docker Hub
    # - task: Docker@2
    #   displayName: Push Docker Image
    #   inputs:
    #     command: push
    #     tags: |
    #       $(imageName):$(tag)

    # 4️⃣ Publish the Dockerfile (and other files) as pipeline artifact
    - task: PublishPipelineArtifact@1
      displayName: 'Publish Dockerfile Artifact'
      inputs:
        targetPath: '$(Build.SourcesDirectory)/Dockerfile'
        artifact: 'dockerfile'
