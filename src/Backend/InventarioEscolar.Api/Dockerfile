# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Stage base para rodar a aplicação
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Stage de build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copia apenas os .csproj para aproveitar cache
COPY ["src/Backend/InventarioEscolar.Api/InventarioEscolar.Api.csproj", "src/Backend/InventarioEscolar.Api/"]
COPY ["src/Shared/InventarioEscolar.Communication/InventarioEscolar.Communication.csproj", "src/Shared/InventarioEscolar.Communication/"]
COPY ["src/Backend/InventarioEscolar.Domain/InventarioEscolar.Domain.csproj", "src/Backend/InventarioEscolar.Domain/"]
COPY ["src/Shared/InventarioEscolar.Exceptions/InventarioEscolar.Exceptions.csproj", "src/Shared/InventarioEscolar.Exceptions/"]
COPY ["src/Backend/InventarioEscolar.Application/InventarioEscolar.Application.csproj", "src/Backend/InventarioEscolar.Application/"]
COPY ["src/Backend/InventarioEscolar.Infrastructure/InventarioEscolar.Infrastructure.csproj", "src/Backend/InventarioEscolar.Infrastructure/"]

# Restaura pacotes
RUN dotnet restore "src/Backend/InventarioEscolar.Api/InventarioEscolar.Api.csproj"

# Copia todo o restante do código
COPY . .

# Corrige WORKDIR para a pasta correta do projeto
WORKDIR "/src/Backend/InventarioEscolar.Api"

# Build do projeto
RUN dotnet build "InventarioEscolar.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Stage de publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "InventarioEscolar.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Stage final
FROM base AS final
ENV ASPNETCORE_ENVIRONMENT=Production
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InventarioEscolar.Api.dll"]
