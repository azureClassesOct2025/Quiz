# ===========================
# Stage 1: Build the app
# ===========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["QuizApp/QuizApp.csproj", "QuizApp/"]
RUN dotnet restore "QuizApp/QuizApp.csproj"

# Copy all project files
COPY . .

# Build and publish the app
WORKDIR "/src/QuizApp"
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# ===========================
# Stage 2: Runtime image
# ===========================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy published output
COPY --from=build /app/publish .

# Expose port
EXPOSE 8080

# Start the application
ENTRYPOINT ["dotnet", "QuizApp.dll"]
