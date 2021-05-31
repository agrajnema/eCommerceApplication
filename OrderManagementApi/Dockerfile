#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["OrderManagementApi/OrderManagementApi.csproj", "OrderManagementApi/"]
COPY ["InfrastructureLibrary/InfrastructureLibrary.csproj", "InfrastructureLibrary/"]
RUN dotnet restore "OrderManagementApi/OrderManagementApi.csproj"
COPY . .
WORKDIR "/src/OrderManagementApi"
RUN dotnet build "OrderManagementApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OrderManagementApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderManagementApi.dll"]