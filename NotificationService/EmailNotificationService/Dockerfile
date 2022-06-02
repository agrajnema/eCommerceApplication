#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["NotificationService/EmailNotificationService/EmailNotificationService.csproj", "NotificationService/EmailNotificationService/"]
COPY ["InfrastructureLibrary/InfrastructureLibrary.csproj", "InfrastructureLibrary/"]
RUN dotnet restore "NotificationService/EmailNotificationService/EmailNotificationService.csproj"
COPY . .
WORKDIR "/src/NotificationService/EmailNotificationService"
RUN dotnet build "EmailNotificationService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EmailNotificationService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EmailNotificationService.dll"]