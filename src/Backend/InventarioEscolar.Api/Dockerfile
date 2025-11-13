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
COPY ["src/Backend/InventarioEscolar.Api/InventarioEscolar.Api.csproj", "src/Backend/InventarioEscolar.Api/"]
COPY ["src/Shared/InventarioEscolar.Communication/InventarioEscolar.Communication.csproj", "src/Shared/InventarioEscolar.Communication/"]
COPY ["src/Backend/InventarioEscolar.Domain/InventarioEscolar.Domain.csproj", "src/Backend/InventarioEscolar.Domain/"]
COPY ["src/Shared/InventarioEscolar.Exceptions/InventarioEscolar.Exceptions.csproj", "src/Shared/InventarioEscolar.Exceptions/"]
COPY ["src/Backend/InventarioEscolar.Application/InventarioEscolar.Application.csproj", "src/Backend/InventarioEscolar.Application/"]
COPY ["src/Backend/InventarioEscolar.Infrastructure/InventarioEscolar.Infrastructure.csproj", "src/Backend/InventarioEscolar.Infrastructure/"]
RUN dotnet restore "./src/Backend/InventarioEscolar.Api/InventarioEscolar.Api.csproj"
COPY . .
WORKDIR "/src/src/Backend/InventarioEscolar.Api"
RUN dotnet build "./InventarioEscolar.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./InventarioEscolar.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
ENV ASPNETCORE_ENVIRONMENT=Production
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InventarioEscolar.Api.dll"]