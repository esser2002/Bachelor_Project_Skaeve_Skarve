# remember to source ~/hdbscan/.venv/bin/activate to enter environment
import scipy.cluster.hierarchy
import numpy
 
from numpy import genfromtxt as gentxt
import csv
with open('/Users/mariehansen/Desktop/Bachelor/Bachelor_Project_Skaeve_Skarve/Code/sDBSCAN/data/out/dendrogram.csv') as csvdatafile:
     # read file as csv file 
    csvreader = csv.reader(csvdatafile, delimiter=";")
    next(csvreader)
    dendrogram = []
    
    # for every row, print the row
    for row in csvreader:
        dendrogram.append ([float(row[0]),float(row[1]),float(row[2]),float(row[3])])
      
    print (dendrogram)    
    

    c,array = scipy.cluster.hierarchy.cophenet(dendrogram,dendrogram)
    print (c)

        
