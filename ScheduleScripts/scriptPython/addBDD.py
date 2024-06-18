from influxdb_client import InfluxDBClient, Point, WritePrecision
from influxdb_client.client.write_api import SYNCHRONOUS
import influxdb_client, os, time
import datetime
url = "http://localhost:8086"  # Remplacez par l'URL de votre instance InfluxDB
token = "Xip2vb7g32N9seLG5mOSynBzRSZuY2zRjyNhcVMyTi407UkamsXkepFZAUP-EXdzfQkCiVYns9m2zQyuU-zBkw=="
org = "stationBus"
bucket = "bucketStation"

write_client = InfluxDBClient(url=url, token=token, org=org)
write_api = write_client.write_api(write_options=SYNCHRONOUS)


def lire_horaires_et_ajouter_dans_influxdb(nom_du_fichier):
    with open(nom_du_fichier, 'r', encoding='utf-8') as fichier:
        lignes = fichier.readlines()
        
        for ligne in lignes:
            # Supposons que la ligne soit formatée comme : "16.101324, 108.139797 : station_name : 08:00, 09:00, 10:00"
            try:
                coords, nom_station, horaires = ligne.strip().split(":", 2)
                latitude, longitude = map(str.strip, coords.split(","))
                horaires_bus = [h.strip() for h in horaires.split(",")]
                
                # Créer un point de données
                point = (
                    Point("bus_stations_infos")
                    .tag("station_name", nom_station.strip())
                    .field("latitude", float(latitude))
                    .field("longitude", float(longitude)))

                # Ajouter chaque horaire comme un champ
                for i, horaire in enumerate(horaires_bus):
                    if i < 9 : 
                        point.field(f"horaire_0{i + 1}", horaire)
                    else :
                        point.field(f"horaire_{i + 1}", horaire)
                
                # Écrire le point dans la base de données
                write_api.write(bucket=bucket, org=org, record=point)
                time.sleep(1)
            except Exception as e:
                print(f"Erreur lors du traitement de la ligne : {ligne.strip()}")
                print(f"Exception: {e}")

# Nom du fichier contenant les horaires et les coordonnées des stations de bus
nom_du_fichier = '../fichiersTxt/output.txt'

# Lire le fichier et ajouter les données dans InfluxDB
lire_horaires_et_ajouter_dans_influxdb(nom_du_fichier)

# Fermer le client
write_client.close()