FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CamundaDotNetWorker/CamundaDotNetWorker.csproj", "CamundaDotNetWorker/"]
RUN dotnet restore "CamundaDotNetWorker/CamundaDotNetWorker.csproj"
COPY . .
WORKDIR "/src/CamundaDotNetWorker"
RUN dotnet build "CamundaDotNetWorker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CamundaDotNetWorker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CamundaDotNetWorker.dll"]
