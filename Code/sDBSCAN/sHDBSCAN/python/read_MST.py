# remember to source ~/hdbscan/.venv/bin/activate to enter environment
import hdbscan 
from numpy import genfromtxt as gentxt
import csv
with open('/home/kassandra/Bachelor_Project_Skaeve_Skarve/Code/sDBSCAN/data/out/mst.csv') as csvdatafile:
     # read file as csv file 
    csvreader = csv.reader(csvdatafile)
    # for every row, print the row
    for row in csvreader:
        print(row)

        
