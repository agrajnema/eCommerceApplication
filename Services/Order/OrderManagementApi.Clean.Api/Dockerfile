#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Services/Order/OrderManagementApi.Clean.Api/OrderManagementApi.Clean.Api.csproj", "Services/Order/OrderManagementApi.Clean.Api/"]
COPY ["Services/Order/OrderManagementApi.Clean.Application/OrderManagementApi.Clean.Application.csproj", "Services/Order/OrderManagementApi.Clean.Application/"]
COPY ["Services/Order/OrderManagementApi.Clean.Domain/OrderManagementApi.Clean.Domain.csproj", "Services/Order/OrderManagementApi.Clean.Domain/"]
COPY ["Services/Order/OrderManagementApi.Clean.Infrastructure/OrderManagementApi.Clean.Infrastructure.csproj", "Services/Order/OrderManagementApi.Clean.Infrastructure/"]
RUN dotnet restore "Services/Order/OrderManagementApi.Clean.Api/OrderManagementApi.Clean.Api.csproj"
COPY . .
WORKDIR "/src/Services/Order/OrderManagementApi.Clean.Api"
RUN dotnet build "OrderManagementApi.Clean.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OrderManagementApi.Clean.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderManagementApi.Clean.Api.dll"]