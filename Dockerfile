# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy everything and publish the release version
COPY ["SpotPriceBridge.csproj", "./"]
COPY ["Controllers/", "Controllers/"]
COPY ["Models/", "Models/"]
COPY ["Program.cs", "./"]
COPY ["Properties/", "Properties/"]
# Add any other necessary folders/files here — exclude bin/, obj/, publish/, etc.

RUN dotnet publish -c Release -o /out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /out .
EXPOSE 80
ENTRYPOINT ["dotnet", "SpotPriceBridge.dll"]
