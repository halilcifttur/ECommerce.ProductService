# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/ECommerce.ProductService.API/ECommerce.ProductService.API.csproj", "src/ECommerce.ProductService.API/"]
COPY ["src/ECommerce.ProductService.Application/ECommerce.ProductService.Application.csproj", "src/ECommerce.ProductService.Application/"]
COPY ["src/ECommerce.ProductService.Domain/ECommerce.ProductService.Domain.csproj", "src/ECommerce.ProductService.Domain/"]
COPY ["src/ECommerce.ProductService.Shared/ECommerce.ProductService.Shared.csproj", "src/ECommerce.ProductService.Shared/"]
COPY ["src/ECommerce.ProductService.Infrastructure/ECommerce.ProductService.Infrastructure.csproj", "src/ECommerce.ProductService.Infrastructure/"]
RUN dotnet restore "./src/ECommerce.ProductService.API/ECommerce.ProductService.API.csproj"
COPY . .
WORKDIR "/src/src/ECommerce.ProductService.API"
RUN dotnet build "./ECommerce.ProductService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ECommerce.ProductService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ECommerce.ProductService.API.dll"]