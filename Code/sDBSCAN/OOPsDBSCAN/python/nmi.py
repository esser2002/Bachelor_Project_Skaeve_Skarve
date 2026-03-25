import numpy as np
from sklearn.cluster import DBSCAN
from sklearn.metrics.cluster import normalized_mutual_info_score 

our_clusters = np.genfromtxt(r"C:\Users\kassa\Desktop\bachelor\Bachelor_Project_Skaeve_Skarve\Code\sDBSCAN\data\out\sDBSCANclusters.csv",dtype=int)

X = np.array([[1, 2], [2, 2], [2, 3],
              [8, 7], [8, 8], [25, 80]])
clustering = DBSCAN(eps=3, min_samples=2).fit(X)

nmi = normalized_mutual_info_score(our_clusters, clustering.labels_)

print(nmi)