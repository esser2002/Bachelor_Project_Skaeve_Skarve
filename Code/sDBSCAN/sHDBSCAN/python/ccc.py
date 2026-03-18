# remember to source ~/hdbscan/.venv/bin/activate to enter environment
from scipy.cluster.hierarchy import cophenet
from scipy.stats import pearsonr
import numpy
import csv
import pandas
import hdbscan


data = numpy.genfromtxt("/Users/mariehansen/Desktop/Bachelor/Bachelor_Project_Skaeve_Skarve/Code/sDBSCAN/data/out/normalisedDataFashion.csv", delimiter=";")
print (data)      



        
clusterer = hdbscan.HDBSCAN(min_cluster_size=2, gen_min_span_tree=True)
clusterer.fit(data)
singleclustertrue = clusterer.single_linkage_tree_.to_numpy()

dendrogram = []

with open('/Users/mariehansen/Desktop/Bachelor/Bachelor_Project_Skaeve_Skarve/Code/sDBSCAN/data/out/dendrogram.csv') as csvdatafile:
     # read file as csv file 
    csvreader = csv.reader(csvdatafile, delimiter=";")
    next(csvreader)

    #for every row, print the row
    for row in csvreader:
        dendrogram.append ([float(row[0]),float(row[1]),float(row[2]),float(row[3])])


cd_approx = cophenet(dendrogram)
cd_true = cophenet(singleclustertrue)

ccc = pearsonr(cd_true, cd_approx)[0]

mr = numpy.exp(numpy.mean(numpy.log(cd_approx / cd_true)))

print("CCC: ", ccc)
print("MR: ", mr)
 
 
 

