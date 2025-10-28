# Use the official .NET 8.0 runtime as the base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use the official .NET 8.0 SDK for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["Quiz/QuizApp/QuizApp.csproj", "Quiz/QuizApp/"]
RUN dotnet restore "Quiz/QuizApp/QuizApp.csproj"

# Copy the rest of the source code
COPY . .

# Build the application
WORKDIR "/src/Quiz/QuizApp"
RUN dotnet build "QuizApp.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "QuizApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Create the final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set the entry point
ENTRYPOINT ["dotnet", "QuizApp.dll"]
