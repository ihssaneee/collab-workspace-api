FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build 
WORKDIR /app


COPY src/CollabWorkspace.Api/*.csproj ./src/CollabWorkspace.Api/
COPY src/CollabWorkspace.Infrastructure/*.csproj ./src/CollabWorkspace.Infrastructure/
COPY src/CollabWorkspace.Domain/*.csproj ./src/CollabWorkspace.Domain/

RUN dotnet restore ./src/CollabWorkspace.Api/*.csproj

COPY src/CollabWorkspace.Api/. ./src/CollabWorkspace.Api/
COPY src/CollabWorkspace.Infrastructure/. ./src/CollabWorkspace.Infrastructure/
COPY src/CollabWorkspace.Domain/. ./src/CollabWorkspace.Domain/

RUN dotnet publish ./src/CollabWorkspace.Api/*.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/out  .

ENTRYPOINT ["dotnet", "CollabWorkspace.Api.dll"]

