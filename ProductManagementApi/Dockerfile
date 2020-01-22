FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as build
EXPOSE 80
EXPOSE 443
WORKDIR /build

# Copy necessary files and restore as distinct layer
#COPY "eCommerceApplication.sln" "eCommerceApplication.sln"
COPY "ProductManagementApi/ProductManagementApi.csproj" "ProductManagementApi/ProductManagementApi.csproj"
COPY "InfrastructureLibrary/InfrastructureLibrary.csproj" "InfrastructureLibrary/InfrastructureLibrary.csproj"
#COPY *.csproj ./
RUN dotnet restore "ProductManagementApi/ProductManagementApi.csproj"
RUN dotnet restore "InfrastructureLibrary/InfrastructureLibrary.csproj"

# Copy everything else and build
COPY . .
RUN dotnet publish -c Release -o output

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build /build/output .

ENTRYPOINT [ "dotnet", "ProductManagementApi.dll"]