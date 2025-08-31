# Use the official ASP.NET Core runtime as a base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Use the .NET SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["Propelo/Propelo.csproj", "Propelo/"]
RUN dotnet restore "Propelo/Propelo.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/Propelo"
RUN dotnet build "Propelo.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "Propelo.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage: runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Propelo.dll"]
