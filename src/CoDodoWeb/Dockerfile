FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER root

RUN apt-get update \
    && apt-get install -y curl jq \
    && rm -rf /var/lib/apt/lists/*
    
USER $APP_UID
WORKDIR /app
     
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/CoDodoWeb/CoDodoWeb.csproj", "src/CoDodoWeb/"]
RUN dotnet restore "src/CoDodoWeb/CoDodoWeb.csproj"
COPY . .
WORKDIR "/src/src/CoDodoWeb"
RUN dotnet build "./CoDodoWeb.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CoDodoWeb.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CoDodoWeb.dll"]
