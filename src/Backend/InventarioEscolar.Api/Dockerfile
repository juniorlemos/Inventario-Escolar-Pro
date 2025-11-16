# Base image para rodar a aplicação
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Stage para build da aplicação
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copia apenas os arquivos .csproj para restaurar dependências
COPY ["Backend/InventarioEscolar.Api/InventarioEscolar.Api.csproj", "InventarioEscolar.Api/"]
COPY ["Backend/InventarioEscolar.Application/InventarioEscolar.Application.csproj", "Backend/InventarioEscolar.Application/"]
COPY ["Backend/InventarioEscolar.Domain/InventarioEscolar.Domain.csproj", "Backend/InventarioEscolar.Domain/"]
COPY ["Backend/InventarioEscolar.Infrastructure/InventarioEscolar.Infrastructure.csproj", "Backend/InventarioEscolar.Infrastructure/"]
COPY ["Shared/InventarioEscolar.Communication/InventarioEscolar.Communication.csproj", "Shared/InventarioEscolar.Communication/"]
COPY ["Shared/InventarioEscolar.Exceptions/InventarioEscolar.Exceptions.csproj", "Shared/InventarioEscolar.Exceptions/"]

# Restaura pacotes
RUN dotnet restore "InventarioEscolar.Api/InventarioEscolar.Api.csproj"

# Copia todo o restante do código
COPY . .

# Build do projeto
RUN dotnet build "Backend/InventarioEscolar.Api/InventarioEscolar.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publicação
FROM build AS publish
RUN dotnet publish "Backend/InventarioEscolar.Api/InventarioEscolar.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Container final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "InventarioEscolar.Api.dll"]
