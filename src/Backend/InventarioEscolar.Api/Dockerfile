# Base image para rodar a aplicação
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Stage para build da aplicação
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copia somente csproj para restaurar dependências
COPY ["src/Backend/InventarioEscolar.Api/InventarioEscolar.Api.csproj", "src/Backend/InventarioEscolar.Api/"]
COPY ["src/Backend/InventarioEscolar.Application/InventarioEscolar.Application.csproj", "src/Backend/InventarioEscolar.Application/"]
COPY ["src/Backend/InventarioEscolar.Domain/InventarioEscolar.Domain.csproj", "src/Backend/InventarioEscolar.Domain/"]
COPY ["src/Backend/InventarioEscolar.Infrastructure/InventarioEscolar.Infrastructure.csproj", "src/Backend/InventarioEscolar.Infrastructure/"]
COPY ["src/Shared/InventarioEscolar.Communication/InventarioEscolar.Communication.csproj", "src/Shared/InventarioEscolar.Communication/"]
COPY ["src/Shared/InventarioEscolar.Exceptions/InventarioEscolar.Exceptions.csproj", "src/Shared/InventarioEscolar.Exceptions/"]

# Restore
RUN dotnet restore "src/Backend/InventarioEscolar.Api/InventarioEscolar.Api.csproj"

# Copia tudo
COPY . .

# Build
RUN dotnet build "src/Backend/InventarioEscolar.Api/InventarioEscolar.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "src/Backend/InventarioEscolar.Api/InventarioEscolar.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Runtime
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "InventarioEscolar.Api.dll"]
