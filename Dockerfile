# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy project file and restore as distinct layers
COPY ["SpotPriceBridge.csproj", "./"]

# Copy all folders used by the app
COPY ["Controllers/", "Controllers/"]
COPY ["Models/", "Models/"]
COPY ["Data/", "Data/"]         
COPY ["Properties/", "Properties/"]

# Copy the main program file
COPY ["Program.cs", "./"]

# Restore and publish
RUN dotnet restore
RUN dotnet publish -c Release -o /out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /out .
EXPOSE 80
ENTRYPOINT ["dotnet", "SpotPriceBridge.dll"]
