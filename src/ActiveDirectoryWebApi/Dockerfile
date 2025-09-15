FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/ActiveDirectoryWebApi/ActiveDirectoryWebApi.csproj", "src/ActiveDirectoryWebApi/"]
RUN dotnet restore "./src/ActiveDirectoryWebApi/./ActiveDirectoryWebApi.csproj"
COPY . .

ARG TARGETPLATFORM

RUN if [ "$TARGETPLATFORM" = "linux/amd64" ]; then \
    RID=linux-x64 ; \
    elif [ "$TARGETPLATFORM" = "linux/arm64" ]; then \
    RID=linux-arm64 ; \
    elif [ "$TARGETPLATFORM" = "linux/arm/v7" ]; then \
    RID=linux-arm ; \
    fi \
    && dotnet publish "src/ActiveDirectoryWebApi/ActiveDirectoryWebApi.csproj" -c Release -o /app/publish -r $RID --self-contained false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
RUN apt update
RUN apt install -y libldap-2.5-0
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["./ActiveDirectoryWebApi"]