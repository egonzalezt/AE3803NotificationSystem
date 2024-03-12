#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["AE3803Notification/AE3803Notification.csproj", "AE3803Notification/"]
COPY ["AE3803Notification.Domain/AE3803Notification.Domain.csproj", "AE3803Notification.Domain/"]
COPY ["AE3803Notification.Infrastructure/AE3803Notification.Infrastructure.csproj", "AE3803Notification.Infrastructure/"]
RUN dotnet restore "./AE3803Notification/./AE3803Notification.csproj"
COPY . .
WORKDIR "/src/AE3803Notification"
RUN dotnet build "./AE3803Notification.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AE3803Notification.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AE3803Notification.dll"]