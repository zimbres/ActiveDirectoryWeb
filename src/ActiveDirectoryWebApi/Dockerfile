FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
RUN apt update && apt install -y libldap-2.5-0
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/ActiveDirectoryWebApi/ActiveDirectoryWebApi.csproj", "src/ActiveDirectoryWebApi/"]
RUN dotnet restore "./src/ActiveDirectoryWebApi/ActiveDirectoryWebApi.csproj"
COPY . .
WORKDIR "/src/src/ActiveDirectoryWebApi"
RUN dotnet build "./ActiveDirectoryWebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ActiveDirectoryWebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ActiveDirectoryWebApi.dll"]
