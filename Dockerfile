# Stage 1: Build the app
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project file and restore dependencies
COPY ./Moebi.Bot/Moebi.Bot.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY ./Moebi.Bot/ ./
RUN dotnet publish -c release -o /app/publish

# Stage 2: Create a runtime image
FROM mcr.microsoft.com/dotnet/runtime:9.0
WORKDIR /app
COPY --from=build /app/publish .

# Set the startup command
ENTRYPOINT ["dotnet", "Moebi.Bot.dll"]
