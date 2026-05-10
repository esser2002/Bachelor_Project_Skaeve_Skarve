import numpy as np
from sklearn.cluster import DBSCAN
from sklearn.metrics.cluster import normalized_mutual_info_score 
import os
import time

# load parameters from da .bat file
pathToOut, sDBSCAN_stats_path, rawdata, normalizeddata, sDBSCAN_clusters, epsilon, minPts = input().split()
normalized_data = np.genfromtxt(pathToOut+normalizeddata, delimiter=";", skip_header=1)
raw_data = np.genfromtxt(rawdata, delimiter=",", skip_header=1)
sDBSCAN = np.genfromtxt(sDBSCAN_clusters,dtype=int)

#
previously_stored_DBSCAN_path = pathToOut+r"\ground_truth_previously_stored_dbscan"
if not os.path.exists(previously_stored_DBSCAN_path):
    with open(previously_stored_DBSCAN_path, "w") as file:
        file.write("Nothing")

f = open(previously_stored_DBSCAN_path)
previous_dbscan = f.read()
f.close()

if(str(previous_dbscan) != str(epsilon)): #Whether to compute the dbscan or use a saved one.
    # Perform SCIKIT DBSCAN
    start = time.perf_counter()

    clustering = DBSCAN(eps=float(epsilon), min_samples=int(minPts), metric='cosine').fit(normalized_data)
    clustering_labels = clustering.labels_
    end = time.perf_counter()
    seconds = (end - start)
    np.save(pathToOut+r"\DBSCAN", clustering_labels)
    np.save(pathToOut+r"\DBSCANTime", seconds)
    f = open(previously_stored_DBSCAN_path, "w")
    f.write(str(epsilon))
    f.close()
    print("saved new DBSCAN")
else:
    clustering_labels = np.load(pathToOut+r"\DBSCAN.npy")
    seconds = np.load(pathToOut+r"\DBSCANTime.npy")
    print("used loaded DBSCAN")



# Extract labels from raw data
ground_truth = [row[0] for row in raw_data]

# Perform OUR nmi to ground truth
sDBSCAN_nmi = normalized_mutual_info_score(sDBSCAN, ground_truth)
#DBSCAN nmi to ground truth
DBSCAN_nmi = normalized_mutual_info_score(clustering_labels, ground_truth)


# Print results
cl = {}
print("DBSCAN clusters:")
for elem in clustering_labels:
    if elem not in cl:
        cl[int(elem)] = 1
    else:
        cl[int(elem)] = cl[int(elem)] + 1

if -1 in cl.keys(): DBSCAN_clusters = len(cl) - 1
else: DBSCAN_clusters = len(cl)
print(cl)
cl = {}

print(DBSCAN_clusters)
print("sDBSCAN clusters")
for elem in sDBSCAN:
    if elem not in cl:
        cl[int(elem)] = 1
    else:
        cl[int(elem)] = cl[int(elem)] + 1

if -1 in cl.keys(): sDBSCAN_clusters = len(cl) - 1
else: sDBSCAN_clusters = len(cl)


print(cl)
print(f"sDBSCAN nmi : {(100 * sDBSCAN_nmi):.2f}%")
print(f"DBSCAN nmi : {(100 * DBSCAN_nmi):.2f}%")

sDBSCANStats = np.genfromtxt(sDBSCAN_stats_path, delimiter=",",dtype=str)

output = (','.join(map(str, np.append(sDBSCANStats, [seconds, sDBSCAN_nmi, DBSCAN_nmi, sDBSCAN_clusters,DBSCAN_clusters]))))
print(output)

#write output in format D,l,m,epsilon,minPts,datasize,sDBSCANtime,DBSCANtime,sDBSCAN_nmi, DSBCAN_nmi, sDBSCAN_clusters, DBSCAN_clusters
with open(pathToOut+r"\dbscan_groundtruth_train_results.csv", "a") as f:
    f.write(output+'\n')