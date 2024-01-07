FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Friday.ERP.Server/Friday.ERP.Server.csproj", "Friday.ERP.Server/"]
COPY ["Friday.ERP.Infrastructure/Friday.ERP.Infrastructure.csproj", "Friday.ERP.Infrastructure/"]
COPY ["Friday.ERP.Core/Friday.ERP.Core.csproj", "Friday.ERP.Core/"]
COPY ["Friday.ERP.Shared/Friday.ERP.Shared.csproj", "Friday.ERP.Shared/"]
RUN dotnet restore "Friday.ERP.Server/Friday.ERP.Server.csproj"
COPY . .
WORKDIR "/src/Friday.ERP.Server"
RUN dotnet build "Friday.ERP.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Friday.ERP.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Friday.ERP.Server.dll"]
