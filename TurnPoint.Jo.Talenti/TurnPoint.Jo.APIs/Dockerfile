# Base runtime environment
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build environment
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy all project files to allow restoring dependencies
COPY ["TurnPoint.Jo.APIs/TurnPoint.Jo.APIs.csproj", "TurnPoint.Jo.APIs/"]
COPY ["TernPoint.Jo.Talenti.DatabaseManager/TernPoint.Jo.Talenti.DatabaseManager.csproj", "TernPoint.Jo.Talenti.DatabaseManager/"]
COPY ["TurnPoint.Jo.Talenti.SharedModels/TurnPoint.Jo.Talenti.SharedModels.csproj", "TurnPoint.Jo.Talenti.SharedModels/"]

# Restore dependencies
RUN dotnet restore "TurnPoint.Jo.APIs/TurnPoint.Jo.APIs.csproj"

# Copy the entire solution
COPY . .

# Set the working directory to the main project
WORKDIR "/src/TurnPoint.Jo.APIs"

# Build the project
RUN dotnet build "TurnPoint.Jo.APIs.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TurnPoint.Jo.APIs.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Runtime environment
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "TurnPoint.Jo.APIs.dll"]
