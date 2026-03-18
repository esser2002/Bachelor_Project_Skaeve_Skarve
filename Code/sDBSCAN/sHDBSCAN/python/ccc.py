# remember to source ~/hdbscan/.venv/bin/activate to enter environment
from scipy.cluster.hierarchy import cophenet
from scipy.stats import pearsonr
import numpy
import csv
from sklearn.datasets import fetch_openml
import hdbscan

X, y = fetch_openml("Fashion-MNIST", version=1, return_X_y=True, as_frame=False)

X_test = X[60000:]
y_test = y[60000:]

X_test.reshape(len(X_test), -1)

clusterer = hdbscan.HDBSCAN(min_cluster_size=2, gen_min_span_tree=True)
clusterer.fit(X_test)
singleyay = clusterer.single_linkage_tree_.to_numpy()

dendrogram = []

with open('/Users/mariehansen/Desktop/Bachelor/Bachelor_Project_Skaeve_Skarve/Code/sDBSCAN/data/out/dendrogram.csv') as csvdatafile:
     # read file as csv file 
    csvreader = csv.reader(csvdatafile, delimiter=";")
    next(csvreader)

    #for every row, print the row
    for row in csvreader:
        dendrogram.append ([float(row[0]),float(row[1]),float(row[2]),float(row[3])])

print (dendrogram)
clusterer

cd_approx = cophenet(dendrogram)
cd_true = cophenet(singleyay)

ccc = pearsonr(cd_true, cd_approx)[0]

mr = numpy.exp(numpy.mean(numpy.log(cd_approx / cd_true)))

print("CCC: ", ccc)
print("MR: ", mr)
 
 
 

