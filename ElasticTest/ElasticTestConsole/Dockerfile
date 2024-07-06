# Use the official Microsoft .NET 8 image as base
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER app
WORKDIR /app

# Build my console application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ElasticTestConsole/ElasticTestConsole.csproj", "ElasticTestConsole/"]
RUN dotnet restore "./ElasticTestConsole/./ElasticTestConsole.csproj"
COPY . .
WORKDIR "/src/ElasticTestConsole"
RUN dotnet build "./ElasticTestConsole.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ElasticTestConsole.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ElasticTestConsole.dll"]