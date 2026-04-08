import numpy as np
from sklearn.cluster import DBSCAN
from sklearn.metrics.cluster import normalized_mutual_info_score 

pathToOut = r"C:\Users\jonas\OneDrive\Dokumenter\ITU\Bachelor_Project_Skæve_Skarve\Code\sDBSCAN\data\out" #Set this to your personal path

sDBSCAN = np.genfromtxt(pathToOut+r"\sDBSCANclusters.csv",dtype=int)
normalized_data = np.genfromtxt(pathToOut+r"\normalisedDataFashion.csv", delimiter=";", skip_header=1)
clustering = DBSCAN(eps=0.08, min_samples=50, metric='cosine').fit(normalized_data)

nmi = normalized_mutual_info_score(sDBSCAN, clustering.labels_)


cl = {}
print("DBSCAN clusters:")
for elem in clustering.labels_:
    if elem not in cl:
        cl[int(elem)] = 1
    else:
        cl[int(elem)] = cl[int(elem)] + 1
print(cl)
cl = {}
print("sDBSCAN clusters")
for elem in sDBSCAN:
    if elem not in cl:
        cl[int(elem)] = 1
    else:
        cl[int(elem)] = cl[int(elem)] + 1

print(cl)
print(f"nmi : {(100 * nmi):.2f}%")