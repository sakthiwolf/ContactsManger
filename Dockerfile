# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0-preview AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:9.0-preview AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files
COPY ["ContactsManger.UI/ContactsManger.UI.csproj", "ContactsManger.UI/"]
COPY ["ContactsMangaer.Core/ContactsMangaer.Core.csproj", "ContactsMangaer.Core/"]
COPY ["ContactsMangaer.Infrastructure/ContactsMangaer.Infrastructure.csproj", "ContactsMangaer.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "ContactsManger.UI/ContactsManger.UI.csproj"

# Copy all source code
COPY . .

# Set working directory to UI project and build
WORKDIR "/src/ContactsManger.UI"
RUN dotnet build "ContactsManger.UI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the app
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ContactsManger.UI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Start the application
ENTRYPOINT ["dotnet", "ContactsManger.UI.dll"]
