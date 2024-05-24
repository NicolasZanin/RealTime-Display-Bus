FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY certificate.pfx /app/certificate.pfx

COPY --from=build-env /app/out .

EXPOSE 8080
EXPOSE 8081

# D�finir la commande de d�marrage par d�faut
ENTRYPOINT ["dotnet", "Server_csharp_uplink.dll"]
