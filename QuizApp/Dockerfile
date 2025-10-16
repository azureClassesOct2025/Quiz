# =============================
# 1️⃣ BUILD STAGE
# =============================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy only project file first (for efficient layer caching)
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# =============================
# 2️⃣ RUNTIME STAGE
# =============================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy the published output from build stage
COPY --from=build /app/out ./

# Expose port 8080 (change if needed)
EXPOSE 8080

# Set the entry point
ENTRYPOINT ["dotnet", "QuizApp.dll"]
