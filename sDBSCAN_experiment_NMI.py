import math

import matplotlib.pyplot as plt
import pandas as pd
import numpy as np


rawdata = pd.read_csv("/Users/mariehansen/Desktop/Bachelor/Bachelor_Project_Skaeve_Skarve/Code/BA_Scalable_Clustering/data/results/dbscan_test_nmi_comparison.csv")
print(rawdata)
averagedata = rawdata.groupby(['m','k']).mean()
print(averagedata)
averagedata.to_csv("/Users/mariehansen/Desktop/Bachelor/Bachelor_Project_Skaeve_Skarve/Code/BA_Scalable_Clustering/data/results/dbscan_test_nmi_comparison_average.csv")

