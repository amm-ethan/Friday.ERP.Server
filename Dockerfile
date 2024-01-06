FROM mcr.microsoft.com/dotnet/aspnet:8.0.0-jammy-chiseled-amd64 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["friday.ERP.Server/friday.ERP.Server.csproj", "friday.ERP.Server/"]
COPY ["friday.ERP.Infrastructure/friday.ERP.Infrastructure.csproj", "friday.ERP.Infrastructure/"]
COPY ["friday.ERP.Core/friday.ERP.Core.csproj", "friday.ERP.Core/"]
COPY ["friday.ERP.Shared/friday.ERP.Shared.csproj", "friday.ERP.Shared/"]
RUN dotnet restore "friday.ERP.Server/friday.ERP.Server.csproj"
COPY . .
WORKDIR "/src/friday.ERP.Server"
RUN dotnet build "friday.ERP.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "friday.ERP.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "friday.ERP.Server.dll"]

#RUN apt-get update && \
#    apt-get install -y libfontconfig1 libgdiplus && \
#    rm -rf /var/lib/apt/lists/* \