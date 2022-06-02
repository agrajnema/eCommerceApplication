#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Product/ProductManagementApi/ProductManagementApi.csproj", "Product/ProductManagementApi/"]
COPY ["InfrastructureLibrary/InfrastructureLibrary.csproj", "InfrastructureLibrary/"]
RUN dotnet restore "Product/ProductManagementApi/ProductManagementApi.csproj"
COPY . .
WORKDIR "/src/Product/ProductManagementApi"
RUN dotnet build "ProductManagementApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProductManagementApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProductManagementApi.dll"]