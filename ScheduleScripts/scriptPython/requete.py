import requests

#Fichier pour récupérer le temps de trajet entre 2 stations
#Le temps de trajet est majoré de 1,5 min pour simuler un possible arret ou un possible ralentissement du traffic

api_key = '84b99f7b-02b8-405f-a3e6-999b6bfd0315'

# Liste des points avec leurs coordonnées (latitude, longitude)
points = {
    "Chung Cư Hòa Hiệp Nam": (16.108574, 108.132814),
    "Đối Diện Bảng Cấm Tắm Biển Nguyễn Tất Thành": (16.101324, 108.139797),
    "Đối Diện Nhà Hàng Japan Nguyễn Tất Thành": (16.094982, 108.146029),
    "Đối diện 136": (16.090842, 108.148456),
    "Số 421 Nguyễn Lương Bằng": (16.086465, 108.144305),
    "Số 339 Nguyễn Lương Bằng": (16.083088, 108.145777),
    "Số 191 Nguyễn Lương Bằng": (16.079631, 108.147143),
    "Số 143 Nguyễn Lương Bằng": (16.076229, 108.148538),
    "Số 27 Nguyễn Lương Bằng": (16.072527, 108.150054),
    "Số 755 - 757": (16.071521, 108.150466),
    "Đối diện 49": (16.070386, 108.153868),
    "Đối diện 316": (16.072397, 108.156687),
    "Kế trụ 504": (16.074691, 108.158227),
    "BV Ung Bướu 1 Hoàng Thị Loan": (16.071348, 108.164298),
    "Đối Diện 92 Phùng Hưng": (16.073173, 108.166542),
    "Chung cư khu CT": (16.074939, 108.169152),
    "35-37": (16.072445, 108.172967),
    "59-61 Lý Thái Tông": (16.074500, 108.175363),
    "BV Da Liễu Dũng Sĩ Thanh Khê": (16.074162, 108.176764),
    "44 Dũng Sĩ Thanh Khê - Dũng Sĩ Thanh Khê": (16.071937, 108.178799),
    "Đối Diện ĐH TDTT Dũng Sĩ Thanh Khê": (16.070699, 108.179903),
    "Số 583 Trần Cao Vân": (16.071368, 108.187993),
    "Ubnd Phường Xuân Hà Trần Cao Vân": (16.071216, 108.192406),
    "Số 301 Trần Cao Vân": (16.071074, 108.198425),
    "Số 189 Trần Cao Vân": (16.071128, 108.201997),
    "Đối Diện 160 Trần Cao Vân": (16.072368, 108.208211),
    "Đối Diện 206 Quang Trung": (16.073388, 108.213178),
    "Đối Diện 120 - BV Đa Khoa Quang Trung": (16.074144, 108.216893),
    "Số 126 Lê Lợi": (16.074382, 108.219954),
    "Số 154 Lê Lợi": (16.072234, 108.220245),
    "Kế Kho Bạc Nhà Nước": (16.071627, 108.223265),
    "Đối Diện Bùng Binh Cầu Sông Hàn Phạm Văn Đồng": (16.072410, 108.232120),
    "Số 124 - Vian Hotel Phạm Văn Đồng": (16.070896, 108.235592),
    "Đối Diện Sân Bóng Đá Harmony Phạm Văn Đồng": (16.069992, 108.240501),
    "Công Viên Biển Đông Võ Nguyên Giáp": (16.073393, 108.245352)
}
# URL de l'API GraphHopper
url = 'https://graphhopper.com/api/1/route'

# Fonction pour envoyer une requête à GraphHopper et obtenir l'itinéraire
def get_route(start, end, api_key):
    params = {
        'point': [f'{start[0]},{start[1]}', f'{end[0]},{end[1]}'],
        'vehicle': 'car',  # Vous pouvez changer le type de véhicule (foot, bike, etc.)
        'locale': 'fr',
        'instructions': 'true',
        'calc_points': 'true',
        'key': api_key
    }
    response = requests.get(url, params=params)
    if response.status_code == 200:
        return response.json()
    else:
        print(f"Erreur lors de la requête: {response.status_code} {response.text}")
        return None

# Liste des noms de points pour conserver l'ordre
point_names = list(points.keys())

# Boucle sur chaque paire de points successifs
with open("../fichiersTxt/itineraire.txt", "w", encoding="utf-8") as f:
    # Boucle sur chaque paire de points successifs
    for i in range(len(point_names) - 1):
        start_name = point_names[i]
        end_name = point_names[i + 1]
        start_point = points[start_name]
        end_point = points[end_name]
        f.write(f"Calcul de l'itinéraire de {start_name} à {end_name}...\n")
        route = get_route(start_point, end_point, api_key)
        if route:
            for path in route['paths']:
                time = round(((path['time']/1000)/60) +1.5)
                f.write(f"Temps: {time} min\n")
            f.write("\n")