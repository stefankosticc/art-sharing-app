﻿FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ArtSharingApp.Backend/ArtSharingApp.Backend.csproj", "ArtSharingApp.Backend/"]
RUN dotnet restore "ArtSharingApp.Backend/ArtSharingApp.Backend.csproj"
COPY . .
WORKDIR "/src/ArtSharingApp.Backend"
RUN dotnet build "ArtSharingApp.Backend.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ArtSharingApp.Backend.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ArtSharingApp.Backend.dll"]
