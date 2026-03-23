from scipy.cluster.hierarchy import cophenet
from scipy.stats import pearsonr
import numpy
import hdbscan
import time

# C:\Users\kassa\Desktop\bachelor\Bachelor_Project_Skaeve_Skarve\Code\sDBSCAN\data\out\normalisedDataFashion.csv
# /Users/mariehansen/Desktop/Bachelor/Bachelor_Project_Skaeve_Skarve/Code/sDBSCAN/data/out/normalisedDataFashion.csv
normalized_data = numpy.genfromtxt(r"C:\Users\kassa\Desktop\bachelor\Bachelor_Project_Skaeve_Skarve\Code\sDBSCAN\data\out\normalisedDataFashion.csv", delimiter=";", skip_header=1)

# C:\Users\kassa\Desktop\bachelor\Bachelor_Project_Skaeve_Skarve\Code\sDBSCAN\data\out\dendrogram.csv
# /Users/mariehansen/Desktop/Bachelor/Bachelor_Project_Skaeve_Skarve/Code/sDBSCAN/data/out/dendrogram.csv
approx_dendrogram = numpy.genfromtxt(r"C:\Users\kassa\Desktop\bachelor\Bachelor_Project_Skaeve_Skarve\Code\sDBSCAN\data\out\dendrogram.csv", delimiter=";", skip_header=1)

start = time.perf_counter()
clusterer = hdbscan.HDBSCAN(min_cluster_size = 30, gen_min_span_tree=True)
clusterer.fit(normalized_data)
true_dendrogram = clusterer.single_linkage_tree_.to_numpy()
end = time.perf_counter()


print("normalized_data:", normalized_data.shape)
print("approx_dendrogram:", len(approx_dendrogram))
print("true_dendrogram:", true_dendrogram.shape)

cd_approx = cophenet(approx_dendrogram)
cd_true = cophenet(true_dendrogram)
ccc = pearsonr(cd_true, cd_approx)[0]

#help
#result = np.exp(np.mean(np.log10(fake_true / fake_app)))
#mr = numpy.exp(numpy.mean(numpy.log(cd_approx / cd_true)))

print("CCC: ", ccc)
print(f"HDBSCAN time: {(end - start)/60:.6f} minutes")

sHDBSCANStats = numpy.genfromtxt(r"C:\Users\kassa\Desktop\bachelor\Bachelor_Project_Skaeve_Skarve\Code\sDBSCAN\data\out\sHDBSCANStats.csv", delimiter=" ",dtype=str)

print(f"sHDBSCAN time: {sHDBSCANStats[4]}")