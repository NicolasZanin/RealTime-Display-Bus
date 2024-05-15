# Utiliser une image de base officielle .NET pour l'environnement de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copier les fichiers .csproj et restaurer les dépendances
COPY *.csproj ./
RUN dotnet restore

# Copier tous les autres fichiers et construire l'application
COPY . ./
RUN dotnet publish -c Release -o out

# Utiliser une image de base plus légère pour exécuter l'application
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Exposer le port sur lequel l'application écoute
EXPOSE 80

# Définir le point d'entrée du conteneur
ENTRYPOINT ["dotnet", "serveur-lorawan.dll"]
